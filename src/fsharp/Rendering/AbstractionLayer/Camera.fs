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
namespace Renderer.AbstractionLayer

module Camera =

    open OpenTK
    open OpenTK.Input

    [<Struct>]
    type Camera = {
        mutable Position : Vec3
        mutable Front : Vec3
        mutable Up : Vec3
        mutable Right : Vec3
        WorldUp : Vec3
        mutable Yaw : float
        mutable Pitch : float
        MovementSpeed : float
        MouseSensitivity : float
        KeyboardSensitivity : float
        Zoom : float
    }

    let updateCameraVectors (camera:byref<Camera>) =
        let yaw, pitch = MathHelper.DegreesToRadians camera.Yaw, MathHelper.DegreesToRadians camera.Pitch
        let fx = (cos yaw) * (cos pitch) |> float32
        let fy = sin pitch |> float32
        let fz = sin yaw * cos pitch |> float32
        camera.Front <- Vec3.Normalize(Vec3(fx, fy, fz))
        camera.Right <- Vec3.Normalize(Vec3.Cross(camera.Front, camera.WorldUp))
        camera.Up <- Vec3.Normalize(Vec3.Cross(camera.Right, camera.Front))
    
    let processKeyboardInput (camera:ref<Camera>) (keyboard:KeyboardDevice) =
        let mutable localCamera = !camera
        let keyboardSensitivity = localCamera.KeyboardSensitivity |> float32
        if keyboard.[Key.W] then
            localCamera.Position <- localCamera.Position + localCamera.Front * keyboardSensitivity
        elif keyboard.[Key.S] then
            localCamera.Position <- localCamera.Position + localCamera.Front * -keyboardSensitivity
        if keyboard.[Key.A] then
            localCamera.Position <- localCamera.Position + localCamera.Right * -keyboardSensitivity
        elif keyboard.[Key.D] then
            localCamera.Position <- localCamera.Position + localCamera.Right * keyboardSensitivity
        if keyboard.[Key.Space] then
            localCamera.Position <- localCamera.Position + localCamera.Up * keyboardSensitivity
        elif keyboard.[Key.C] then
            localCamera.Position <- localCamera.Position + localCamera.Up * -keyboardSensitivity
        camera := localCamera

    let processMouseInputConstrained (camera:ref<Camera>) xoffset yoffset =
        let mutable localCamera = !camera
        localCamera.Yaw <- localCamera.Yaw - xoffset * localCamera.MouseSensitivity
        let newPitch =
            let p = localCamera.Pitch + yoffset * localCamera.MouseSensitivity
            if p > 89.0 then 89.0
            elif p < -89.0 then -89.0
            else p
        localCamera.Pitch <- newPitch
        updateCameraVectors &localCamera
        camera := localCamera

    let getViewMatrix camera = Mat4.LookAt(camera.Position, camera.Position + camera.Front, camera.Up)

    let create position up yaw pitch front movementSpeed mouseSensitivity keyboardSensitivity zoom =
        let mutable camera = 
            {   Position = position
                Front = front
                Up = Vec3.Zero
                Right = Vec3.Zero
                WorldUp = up
                Yaw = yaw
                Pitch = pitch
                MovementSpeed = movementSpeed
                MouseSensitivity = mouseSensitivity
                KeyboardSensitivity = keyboardSensitivity
                Zoom = zoom }
        updateCameraVectors &camera
        camera

    let createDefault () =
        create (Vec3.Zero) (Vec3(0.f, 1.f, 0.f)) -90.0 0. (Vec3(0.f, 0.f, 1.f)) 2.5 0.1 0.005 45.