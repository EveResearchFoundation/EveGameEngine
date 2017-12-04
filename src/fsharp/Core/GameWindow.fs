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
namespace Core

open System

open OpenTK
open OpenTK.Input

open Renderer
open Renderer.AbstractionLayer
open Renderer.GUI.Widget

type IGameWindow =
    abstract member Visible : bool with get, set
    abstract member Size : Drawing.Size with get, set
    abstract member Run : unit -> unit
    abstract member Exit : unit -> unit


type GameWindowGL(scene, width, height, options) as self =
    
    let camera = Camera.createDefault()
    
    let openGLRenderWindow = new Renderer.OpenGL4Renderer(scene, camera, width, height, options)
    do
        let window = openGLRenderWindow
        window.MouseMove.Add(fun args ->
            if window.Mouse.[MouseButton.Left] then
                let dx = -args.XDelta |> float
                let dy = -args.YDelta |> float
                Camera.processMouseInputConstrained camera dx dy
        )

        window.UpdateFrame.Add(fun _ ->
            let keyboard = window.Keyboard
            Camera.processKeyboardInput camera keyboard
        )
    
    // Exposing necessary events to allow game logic execution per frame

    member __.FocusChanged = openGLRenderWindow.FocusedChanged

    member __.UpdateFrame = openGLRenderWindow.UpdateFrame

    member __.Closing = openGLRenderWindow.Closing

    member __.Closed = openGLRenderWindow.Closed

    member __.RenderFrame = openGLRenderWindow.RenderFrame

    member __.Keyboard = openGLRenderWindow.Keyboard

    //member val Visible = (self :> IGameWindow).Visible

    //member val Size = (self :> IGameWindow).Size

    member __.Run () = (self :> IGameWindow).Run ()

    member __.Exit () = (self :> IGameWindow).Exit ()

    interface IGameWindow with
        member val Visible = openGLRenderWindow.Visible with get, set

        member val Size = openGLRenderWindow.Size with get, set

        member __.Run () = openGLRenderWindow.Run ()

        member __.Exit () = openGLRenderWindow.Exit ()

    interface IDisposable with
        member __.Dispose () =
            if openGLRenderWindow.Visible then
                openGLRenderWindow.Close()
            openGLRenderWindow.Dispose()