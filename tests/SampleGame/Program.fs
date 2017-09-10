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
module SampleGame

// std lib imports
open System

// dependency imports
open OpenTK.Input

// Eve render engine imports
open Renderer.Renderer
open Renderer.AbstractionLayer.Camera
open Renderer.AbstractionLayer
open Renderer

open Support

[<EntryPoint>]
let main args =
    printfn "args: %A" args
    let camera = Camera.createDefault() |> ref
    let mutable nanosuitModel = Manager.loadModel nanosuitPath
    nanosuitModel.Translation <- Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, -0.5f, -0.5f)
    let scene = 
        {   Models = [| nanosuitModel |] 
            Initialized = true  } |> ref

    use window = new OpenGL4Renderer(scene, camera, 800, 600)

    let rec gameLoop lastMouseX lastMouseY =
        if window.IsExiting |> not then
            window.Render()
            window.ProcessEvents()
            if window.Mouse.[MouseButton.Left] then
                let dx = lastMouseX - window.Mouse.X |> float
                let dy = lastMouseY - window.Mouse.Y |> float
                Camera.processMouseInputConstrained camera dx dy
            let mouseX, mouseY = window.Mouse.X, window.Mouse.Y
            System.Threading.Thread.Sleep(1)
            gameLoop mouseX mouseY
    
    gameLoop window.Mouse.X window.Mouse.Y

    Console.ReadKey() |> ignore

    0