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
#nowarn "9"
open System.Runtime.InteropServices


/// <summary>
/// We use OpenTK's vectors as standard vector types accross the whole engine.
/// </summary>
[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type Vertex = {
    Position : Vec3
    Normal : Vec3
    TexCoord : Vec2
}

with

    /// <summary>
    /// Helper function for retrieving the offset of a specific vertex attribute (struct).
    /// </summary>
    /// <param name="name"></param>
    static member offsetof name = 
        Marshal.OffsetOf(typeof<Vertex>, name)
    
    /// <summary>
    /// Helper value indicating the offset of the `Position` field.
    /// </summary>
    static member offset_Position = 
        Vertex.offsetof "Position@"
    
    /// <summary>
    /// Helper value indicating the offset of the `Normal` field.
    /// </summary>
    static member offset_Normal = 
        Vertex.offsetof "Normal@"

    /// <summary>
    /// Helper value indicating the offset of the `TexCoord` field.
    /// </summary>
    static member offset_TexCoord = 
        Vertex.offsetof "TexCoord@"
        
    /// <summary>
    /// Helper value indicating the size of the `Vertex` struct record.
    /// </summary>
    static member size = Marshal.SizeOf<Vertex>()//<Vertex>()