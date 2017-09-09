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
namespace Renderer.LowLevel.GL4
open Renderer
open Renderer.AbstractionLayer

module MeshLLAttachment = 
    open System

    open OpenTK.Graphics.OpenGL4
    
    open Utils
    open Mesh

    /// <summary>
    /// The necessary low level information for a mesh object
    /// </summary>
    [<Struct>]
    type MeshLLAttachment = {
        VAO : int
        VBO : int
        EBO : int
    }

    module MeshLLAttachment =
        
        open Shader

        let create (mesh:Mesh) =
            let vertices, indices = mesh.Vertices, mesh.Indices
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
        
            {   VAO = vao
                VBO = vbo
                EBO = ebo   }
        
        let draw (mesh:Mesh) (attachment:MeshLLAttachment) (ShaderProgram shaderProgramID as program) =
            let material = Renderer.Manager.getMaterial mesh.MaterialReference
            do GL.BindVertexArray attachment.VAO
            do GL.UseProgram shaderProgramID
            do Material.activate program material
            do GL.DrawElements(BeginMode.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, 0)
            do GL.BindVertexArray(0)

    ///// <summary>
    ///// Creates an OpenTK vec3 from an existing Assimp vector3
    ///// </summary>
    ///// <param name="vec3">an vec3 from Assimp</param>
    //let private createOpenTKVector3 (vec3:Assimp.Vector3D) = Vec3(vec3.X, vec3.Y, vec3.Z)
    
    //let draw (mesh:GenericMesh) (ShaderProgram shaderProgramID) = 
    //    do GL.BindVertexArray(mesh.VAO)
    //    do GL.UseProgram shaderProgramID
    //    do GL.DrawElements(BeginMode.Triangles, mesh.BaseData.Indices.Length, DrawElementsType.UnsignedInt, 0)
    //    do GL.BindVertexArray(0)

    ///// <summary>
    ///// Primary function for loading models. Not safe to use _yet_
    ///// </summary>
    ///// <param name="path">The absolute or relative path to the model file</param>
    //let create (scene:Assimp.Scene) =
    //    let model = scene.Meshes
    //    let meshes = model |> Seq.map (fun mesh -> Mesh.createFromAssimpMesh mesh) |> Seq.toList
    //    { Meshes = meshes }