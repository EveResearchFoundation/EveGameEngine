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

    open OpenTK.Graphics.OpenGL4

    open Renderer.LowLevel.GL4
    open Buffer
    open AbstractionLayer.Camera

    type OpenGL4Renderer (scene, camera:byref<Camera<Vec3>>) =
        do
            if scene.Initialized |> not then failwith "FATAL ERROR: Scene was not initialized"

        let scene : Scene = scene
        let camera = camera

        let shader = 
            Shader.create 
                [|  ShaderType.VertexShader,    "../../../../resources/shaders/SimpleLight.vert" 
                    ShaderType.FragmentShader,  "../../../../resources/shaders/SimpleLight.frag" |] 
        
        let models =
            [| for model in scene.Models -> 
                model.Translation, [| for mesh in model.Meshes -> mesh, MeshLLAttachment.MeshLLAttachment.create mesh |]
            |]

        member self.Render () =
            let viewMatrix = getViewMatrix camera 
            Shader.setMat4 "view" viewMatrix shader
            for glModel in models do
                let translation, meshes = glModel
                Shader.setMat4 "worldMatrix" translation shader
                for glMesh in meshes do 
                    let mesh, attachment = glMesh
                    MeshLLAttachment.MeshLLAttachment.draw mesh attachment shader
                ()

