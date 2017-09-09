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
module Material =
    open System

    open Texture
    
    /// <summary>
    /// Contains all minimum information about a material. 
    /// Such includes color behaviour and texture
    /// </summary>
    /// <remark>
    /// Materials should be managed through using the resource manager. Such efficiently manages those aswell.
    /// </remark>
    [<Struct>]
    type GenericMaterial<'Vec4Type, 'Precision when 'Precision : unmanaged> = {
        Ambient : 'Vec4Type
        Diffuse : 'Vec4Type
        Specular : 'Vec4Type
        Emissive : 'Vec4Type
        Shininess : 'Precision
    }

    type MaterialReferenceID = Guid
    
    type Material = GenericMaterial<Vec4, float32>