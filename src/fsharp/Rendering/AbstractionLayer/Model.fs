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

module Model =
    open System

    open Mesh

    [<Struct>]
    type GenericModel<'Vec3Type, 'Vec2Type, 'Mat4Type> = {
        Meshes : GenericMesh<'Vec3Type, 'Vec2Type> []
        mutable Translation : 'Mat4Type
    }

    type Model = GenericModel<Vec3, Vec2, Mat4>

    // NOTE:
    // This should be done somewhere else under IO utilities.

    