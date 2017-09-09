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

module Texture =
    open System

    /// <summary>
    /// Describes future texture usage. 
    /// Primarily used in the resource manager.
    /// </summary>
    type TextureUsageKind =
        | Ambient
        | Diffuse
        | Specular
        | Reflection
        | Custom
    
    /// <summary>
    /// Texture type which describes all base information. Information about usage is discareded for performance.
    /// </summary>
    [<Struct>]
    type Texture2D = {
        Width : int
        Height : int
        Data : uint8[]
    }
    
    /// <summary>
    /// As usual, because Texture is a resource type it can only recieved through the guid from the resource manager. 
    /// For more information see the extended documentation on github.
    /// This type alias should warn the developer that a certain Guid is intended to be used _only_ for usage-cases described above.
    /// </summary>
    type TextureReferenceID = System.Guid

    type TextureContentPair =
            System.Collections.Generic.KeyValuePair<TextureReferenceID, Texture2D>
