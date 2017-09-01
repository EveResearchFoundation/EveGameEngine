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

module Mesh
open Vertex
open Shader

open OpenTK
open OpenTK.Graphics.OpenGL4

[<Struct>]
type Mesh = {
    Vertices : Vertex[]
    Indices : uint32 []
    VAO : int
    VBO : int
    EBO : int
}

let create (vertices:Vertex []) (indices:uint32 []) =
    let vao, vbo, ebo = GL.GenVertexArray(), GL.GenBuffer(), GL.GenBuffer()
    do GL.BindVertexArray vao
    do GL.BindBuffer(BufferTarget.ArrayBuffer, vbo)
    do GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof<Vertex>, vertices, BufferUsageHint.StaticDraw)

    do GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo)
    do GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof<int>, indices, BufferUsageHint.StaticDraw)

    do GL.EnableVertexAttribArray(0)
    do GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vertex.size, Vertex.offset_Position)
    do GL.EnableVertexAttribArray(1)
    do GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, Vertex.size, Vertex.offset_Normal)
    do GL.EnableVertexAttribArray(1)
    do GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, Vertex.size, Vertex.offset_TexCoord)

    do GL.BindVertexArray(0)

    {   Vertices = vertices
        Indices = indices
        VAO = vao
        VBO = vbo
        EBO = ebo   }

/// <summary>
/// Creates an OpenTK vec3 from an existing Assimp vector3
/// </summary>
/// <param name="vec3">an vec3 from Assimp</param>
let private createOpenTKVector3 (vec3:Assimp.Vector3D) = Vec3(vec3.X, vec3.Y, vec3.Z)
        
let createFromAssimpMesh (mesh:Assimp.Mesh) =
    let vertices = 
        [|   for i in 0..(mesh.VertexCount - 1) do
                let pos = createOpenTKVector3 mesh.Vertices.[i]
                let normal = createOpenTKVector3 mesh.Normals.[i]
                yield { Position =  pos; Normal = normal; TexCoord = Vec2(0.f, 1.f)} |]
    let indicies = mesh.GetUnsignedIndices()
    let materialIndex = mesh.MaterialIndex
    create vertices indicies
    
let draw (mesh:Mesh) (ShaderProgram shaderProgramID) = 
    do GL.BindVertexArray(mesh.VAO)
    do GL.UseProgram shaderProgramID
    do GL.DrawElements(BeginMode.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0)
    do GL.BindVertexArray(0)