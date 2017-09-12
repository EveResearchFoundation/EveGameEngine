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
open Renderer.AbstractionLayer.Camera
open Renderer.AbstractionLayer
open Renderer

open Support
open System.Collections.Generic

[<EntryPoint>]
let main args =
    printfn "args: %A" args
    let mutable nanosuitModel, nanosuitGuid = Manager.loadModel nanosuitPath, Guid.NewGuid()
    nanosuitModel.Translation <- Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, -0.5f, -0.5f)
    //let mutable brainStemModel = Manager.loadModel brainStemPath
    // nanosuitModel.Translation <- Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, -0.5f, -0.5f)
    let scene = 
        {   Models = Map [ nanosuitGuid, nanosuitModel ] |> Dictionary
            Initialized = true  } |> ref

    use window = new Core.GameWindowGL(scene, 800, 600)

    let mutable i = 0
    window.UpdateFrame.Add(fun _ ->
        let t = i |> float32
        let pi = Math.PI |> float32
        let newTranslation = Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, 0.5f * sin(t * 2.f * pi * 0.005f), -0.5f)
        scene.Value.Models.[nanosuitGuid].Translation  <- newTranslation
        if i >= 1000 then
            i <- 0
        else
            i <- i + 1
    )
    
    window.Run()

    Console.ReadKey() |> ignore

    0