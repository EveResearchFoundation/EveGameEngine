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
/// Standard vertex definition for the EveGameEngine.
/// </summary>
[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type GenericVertex<'Vec3Type, 'Vec2Type> = {
    Position : 'Vec3Type
    Normal : 'Vec3Type
    TexCoord : 'Vec2Type
}

with
    /// <summary>
    /// Helper function for retrieving the offset of a specific vertex attribute (struct).
    /// </summary>
    /// <param name="name"></param>
    static member offsetof name = 
        Marshal.OffsetOf(typeof<GenericVertex<'Vec3Type, 'Vec2Type>>, name)
    
    /// <summary>
    /// Helper value indicating the offset of the `Position` field.
    /// </summary>
    static member offset_Position = 
        GenericVertex<'Vec3Type, 'Vec2Type>.offsetof "Position@"
    
    /// <summary>
    /// Helper value indicating the offset of the `Normal` field.
    /// </summary>
    static member offset_Normal = 
        GenericVertex<'Vec3Type, 'Vec2Type>.offsetof "Normal@"

    /// <summary>
    /// Helper value indicating the offset of the `TexCoord` field.
    /// </summary>
    static member offset_TexCoord = 
        GenericVertex<'Vec3Type, 'Vec2Type>.offsetof "TexCoord@"
        
    /// <summary>
    /// Helper value indicating the size of the `Vertex` struct record.
    /// </summary>
    static member size = Marshal.SizeOf<GenericVertex<'Vec3Type, 'Vec2Type>>()


/// <summary>
/// We use OpenTK's vectors as standard vector types accross the whole engine.
/// </summary>
type Vertex = GenericVertex<Vec3, Vec2>