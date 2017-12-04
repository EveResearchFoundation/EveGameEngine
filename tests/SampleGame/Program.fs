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
open Utils.Logging

open Support
open System.Collections.Generic

[<EntryPoint>]
let main args =
    printfn "args: %A" args
    let nanosuitModel, nanosuitGuid = Manager.loadModel nanosuitPath, Guid.NewGuid()
    nanosuitModel.Translation <- Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, -0.5f, -0.5f)
    //let mutable brainStemModel = Manager.loadModel brainStemPath
    // nanosuitModel.Translation <- Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, -0.5f, -0.5f)
    let scene = 
        {   Models = Map [ nanosuitGuid, nanosuitModel ] |> Dictionary
            Lights = [| { Pos = Vec3(1.f, 1.f, 2.f); Color = Vec3(10.f, 10.f, 10.f) }|]
            Initialized = true  }

    use window = new Core.GameWindowGL(scene, 800, 600, { HotReloadShaders = true })

    let mutable i = 0
    window.UpdateFrame.Add(fun _ ->
        let t = i |> float32
        let pi = Math.PI |> float32
        let newTranslation = Vec3(2.f * cos(t * 2.f * pi * 0.005f), 1.f, 1.f * sin(t * 2.f * pi * 0.005f)) //Mat4.CreateScale(0.05f) * Mat4.CreateTranslation(0.f, 0.5f * sin(t * 2.f * pi * 0.005f), -0.5f)
        scene.Lights.[0].Pos <- newTranslation
        scene.Lights.[0].Color <- Vec3(1.f * cos(t * 2.f * pi * 0.005f) ** 2.f, 1.f, 1.f * sin(t * 2.f * pi * 0.005f) ** 2.f)
        if i >= 1000 then
            i <- 0
            GC.Collect()
            GC.WaitForFullGCComplete() |> logDebug 
        else
            i <- i + 1
    )
    
    window.Run()
    
    logInfo "Game window was closed. Press any key to exit..."
    Console.ReadKey() |> ignore

    0