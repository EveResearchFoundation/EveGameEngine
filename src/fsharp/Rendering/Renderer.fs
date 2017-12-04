// --------------------------------------------------------------------------
//  Copyright (c) 2017 Victor Peter Rouven Müller
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// --------------------------------------------------------------------------
namespace Renderer

[<RequireQualifiedAccess>]
module Renderer =
    open System

    open OpenTK
    open OpenTK.Graphics.OpenGL4

    open Renderer.LowLevel.GL4

    open AbstractionLayer.Camera
    open Renderer.AbstractionLayer
    open System.IO

    //let private assemblyResources = ResourceManager( ,System.Reflection.Assembly.GetExecutingAssembly())
    
    type RendererOptions = {
        HotReloadShaders : bool
    }

    type OpenGL4Renderer (scene, camera:Camera, width, height, options:RendererOptions) as self =
        inherit GameWindow(width, height, Graphics.GraphicsMode.Default, "F# OpenGL Test", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, Graphics.GraphicsContextFlags.Debug ||| Graphics.GraphicsContextFlags.ForwardCompatible)
        do
            OpenTK.Toolkit.Init(ToolkitOptions.Default) |> ignore
            if scene.Initialized |> not then failwith "FATAL ERROR: Scene was not initialized"
            printfn "Embedded assembly resources %A" (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
            

        let mutable scene = scene
        let camera = camera

        let createProjectionMatrix windowWidth windowHeight =
            Matrix4.CreatePerspectiveFieldOfView(0.785398f, float32(windowWidth / windowHeight), 0.01f, 100.0f) // 0,785398 radians are circa 45 degree

        let mutable projectionMatrix = createProjectionMatrix width height
        
        //let shaderDir = 
        let shaderPaths = [| "SimpleLight.vert"; "SimpleLight.frag" |]
        
        //let recreateShaders =
        //    if options.HotReloadShaders then
        //        (fun () -> ())
        //    else
        //        let unwrap = 
        //            function
        //            | Ok content -> content
        //            | Error e -> failwithf "Loading shaders failed with: %A" e
                
        //        (fun () -> 
        //            Shader.create 
        //                [|  ShaderType.VertexShader,    Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.vert" |> unwrap
        //                    ShaderType.FragmentShader,  Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.frag" |> unwrap |])

        let mutable shader = 
            let unwrap = 
                    function
                    | Ok content -> content
                    | Error e -> failwithf "Loading shaders failed with: %A" e
            Shader.create 
                [|  ShaderType.VertexShader,    Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.vert" |> unwrap
                    ShaderType.FragmentShader,  Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.frag" |> unwrap |]

        //let watchers = 
        //    if options.HotReloadShaders then
        //        let watchers = 
        //            [| for shaderPath in shaderPaths do
        //                let watcher = 
        //                    match shaderPath |> Utils.File.tryGetFullPath with
        //                    | Some path -> FileSystemWatcher path
        //                    | _ -> failwithf "Shaderpath %s is invalid!" shaderPath
        //                watcher.Changed.Add(fun _ -> shader <- recreateShaders ())
        //                yield watcher |]
        //        Some watchers
        //    else None
        
        let models =
            scene.Models
            |> Seq.map (fun model -> model.Key, model.Value.Meshes |> Array.mapi (fun i mesh -> i, MeshLLAttachment.MeshLLAttachment.create mesh))
            |> Seq.toArray

        let render () =
            match GL.GetError() with
            | ErrorCode.NoError -> ()
            | _ as error -> printfn "%A" error
            GL.ClearColor(System.Drawing.Color.Black)
            GL.Clear(ClearBufferMask.ColorBufferBit ||| ClearBufferMask.DepthBufferBit)
            Shader.useProgram shader
            let viewMatrix = getViewMatrix camera
            Shader.setMat4 "view" viewMatrix shader
            Shader.setMat4 "projection" projectionMatrix shader 
            Shader.setVec3 "viewPos" camera.Position shader
            // let glModels = models |> Array.map (fun (key, meshes) -> scene.Models.[key], meshes)
            for light in scene.Lights do
                let lightPos = light.Pos//Vector3(1.0f, 1.0f, 2.0f) 
                Shader.setVec3 "lightPos" lightPos shader    
                Shader.setVec3 "lightColor" light.Color shader
                for glModel in models do
                    let key, meshes = glModel
                    let model = scene.Models.[key]
                    let translation = model.Translation
                    let normalMatrix = Mat4.Transpose translation
                    Shader.setMat4 "model" translation shader
                    Shader.setMat4 "normalModel" normalMatrix shader
                    for glMesh in meshes do 
                        let index, attachment = glMesh
                        let mesh = model.Meshes.[index]
                        MeshLLAttachment.MeshLLAttachment.draw mesh attachment shader
            self.SwapBuffers()
            // translationPass (ref scene)

        let glContext = self.Context
        do
            self.Visible <- true
            glContext.LoadAll()
            glContext.ErrorChecking <- true
            GL.Enable(EnableCap.DepthTest) 
            GL.Viewport(0, 0, width, height)

            self.RenderFrame.Add(fun _ -> render())

            self.TargetUpdateFrequency <- 60.

        override __.OnResize(args:EventArgs) =
            self.MakeCurrent()
            let w, h = self.Width, self.Height
            GL.Viewport(0, 0, w, h)
            projectionMatrix <- createProjectionMatrix w h
            ()

        interface IDisposable with
            override self.Dispose() =
                //watchers |> Option.iter (Array.iter Utils.IDisposable.dispose)
                ()
        

