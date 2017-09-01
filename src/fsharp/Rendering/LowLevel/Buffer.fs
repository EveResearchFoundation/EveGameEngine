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

module Buffer
open OpenTK
open OpenTK.Graphics.OpenGL4

module VAO =
    /// <summary>
    ///  Safe wrapping of the vertex array object's int id. 
    /// </summary>
    [<Struct>]
    type VertexArrayObject = | VertexArrayObject of id:int

    type VAO = VertexArrayObject
    
    /// <summary>
    /// Used for indicating that a specific VAO is being used in a block. However this is not that safe to use, it's for research purposes.
    /// </summary>
    type VertexArrayObjectBuilder(vao:VertexArrayObject) =
        let (VertexArrayObject id) = vao

        member self.Run f =
            // Bind the vertex array object
            GL.BindVertexArray id
            // Call the content in the block
            f ()
            // Unbind our vertex array object
            GL.BindVertexArray 0u
    