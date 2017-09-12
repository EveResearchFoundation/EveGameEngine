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

[<AutoOpen>]
module RendererUtils
type Vec4 = OpenTK.Vector4
type Vec3 = OpenTK.Vector3
type Vec2 = OpenTK.Vector2
type Mat4 = OpenTK.Matrix4
type Mat3 = OpenTK.Matrix3

let inline vec3FromAssimpVec3 (vec3:Assimp.Vector3D) = Vec3(vec3.X, vec3.Y, vec3.Z)
let inline vec4FromAssimpVec4 (vec4:Assimp.Color4D) = Vec4(vec4.R, vec4.G, vec4.B, vec4.A)