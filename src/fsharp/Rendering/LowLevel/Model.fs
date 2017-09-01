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

module Model
open OpenTK
open OpenTK.Graphics.OpenGL4
open Mesh

type Model = {
    Meshes : Mesh list
}

// NOTE:
// This should be done somewhere else under IO utilities.

/// <summary>
/// Primary function for loading models. Not safe to use _yet_
/// </summary>
/// <param name="path">The absolute or relative path to the model file</param>
let create path =
    let absPath = System.IO.Path.GetFullPath path
    use context = new Assimp.AssimpContext()
    // NOTE:
    // We're asuming that the scene contains a single model built from all meshes in it.
    let scene = context.ImportFile path 
    let model = scene.Meshes
    let meshes = model |> Seq.map (fun mesh -> Mesh.createFromAssimpMesh mesh) |> Seq.toList
    { Meshes = meshes }