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

module Renderer =
    open System
    open System.Resources

    open OpenTK
    open OpenTK.Graphics.OpenGL4

    open Renderer.LowLevel.GL4
    open Buffer
    open AbstractionLayer.Camera

    //let private assemblyResources = ResourceManager( ,System.Reflection.Assembly.GetExecutingAssembly())

    type OpenGL4Renderer (scene:ref<Scene>, camera:ref<Camera<Vec3>>, width, height) as self =
        inherit GameWindow(width, height, Graphics.GraphicsMode.Default, "F# OpenGL Test", GameWindowFlags.Default, DisplayDevice.Default, 4, 0, Graphics.GraphicsContextFlags.Debug ||| Graphics.GraphicsContextFlags.ForwardCompatible)
        do
            OpenTK.Toolkit.Init(ToolkitOptions.Default) |> ignore
            if (!scene).Initialized |> not then failwith "FATAL ERROR: Scene was not initialized"
            printfn "Embedded assembly resources %A" (System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()) 

        let scene = scene
        let camera = camera

        let createProjectionMatrix windowWidth windowHeight =
            Matrix4.CreatePerspectiveFieldOfView(0.785398f, float32(windowWidth / windowHeight), 0.01f, 100.0f) // 0,785398 radians are circa 45 degree

        let mutable projectionMatrix = createProjectionMatrix width height
        
        let glContext = self.Context
        do
            self.Visible <- true
            glContext.LoadAll()
            glContext.ErrorChecking <- true
            GL.Enable(EnableCap.DepthTest)
            GL.Viewport(0, 0, width, height)

        let shader = 
            let unwrap = 
                function
                | Ok content -> content
                | Error e -> failwithf "Loading shaders failed with: %A" e

            Shader.create 
                [|  ShaderType.VertexShader,    Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.vert" |> unwrap
                    ShaderType.FragmentShader,  Manager.tryGetContentOfEmbeddedTextFile "SimpleLight.frag" |> unwrap |] 
        
        let models =
            (!scene).Models 
            |> Array.mapi (fun i model -> i, model.Meshes |> Array.mapi (fun i mesh -> i, MeshLLAttachment.MeshLLAttachment.create mesh))
        

        override __.OnResize(args:EventArgs) =
            self.MakeCurrent()
            printfn "resized"
            let w, h = self.Width, self.Height
            GL.Viewport(0, 0, w, h)
            projectionMatrix <- createProjectionMatrix w h

            ()

        member __.Render () =
            match GL.GetError() with
            | ErrorCode.NoError -> ()
            | _ as error -> printfn "%A" error
            GL.ClearColor(System.Drawing.Color.Black)
            GL.Clear(ClearBufferMask.ColorBufferBit ||| ClearBufferMask.DepthBufferBit)
            Shader.useProgram shader
            let viewMatrix = getViewMatrix (!camera)
            Shader.setMat4 "view" viewMatrix shader
            let lightPos = Vector3(1.0f, 1.0f, 2.0f)
            [|  // Shader.setMat4 "model" modelMatrix
                Shader.setMat4 "projection" projectionMatrix
                // Shader.setMat4 "normalModel" normalMatrix
                Shader.setVec3 "lightPos" lightPos
                Shader.setVec3 "viewPos" (!camera).Position |]
            |> Array.iter (fun f -> f shader)
            for glModel in models do
                let i, meshes = glModel
                let model = (!scene).Models.[i]
                let translation = model.Translation
                let normalMatrix = Mat4.Transpose(translation)
                Shader.setMat4 "model" translation shader
                Shader.setMat4 "normalModel" normalMatrix shader
                for glMesh in meshes do 
                    let i, attachment = glMesh
                    let mesh = model.Meshes.[i]
                    MeshLLAttachment.MeshLLAttachment.draw mesh attachment shader
            self.SwapBuffers()

