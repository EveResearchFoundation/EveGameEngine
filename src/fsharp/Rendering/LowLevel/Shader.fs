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

module Shader
open OpenTK
open OpenTK.Graphics.OpenGL4

[<Struct>]
type ShaderProgram = | ShaderProgram of int

/// <summary>
/// Restores the default shader program (per definition it's zero)
/// </summary>
let reset () = GL.UseProgram 0

/// <summary>
/// Returns the location of the uniform from the given shader.
/// </summary>
/// <param name="name">The name of the uniform</param>
let getUniformLocation (ShaderProgram(p)) name =
    GL.GetUniformLocation(p, name)

let useProgram (ShaderProgram(p):ShaderProgram) = GL.UseProgram p

/// <summary>
/// Sets the corresponding mat4 using the shader provided
/// </summary>
/// <param name="name">The name of the mat4 uniform in the shader</param>
/// <param name="value">The new mat4 value</param>
let setMat4 name value (ShaderProgram(p) as program) = 
    let mutable m = value
    let loc = getUniformLocation program name
    useProgram program
    GL.UniformMatrix4(loc, false, &m)
#if DEBUG
    printfn "updated uniform %A with value %A" name value
#endif
    reset ()

/// <summary>
/// Sets the corresponding vec3 using the shader provided
/// </summary>
/// <param name="name">The name of the vec3 uniform in the shader</param>
/// <param name="value">The new vec3 value</param>
let setVec3 name (value:Vec3) (ShaderProgram(p) as program) = 
    let loc = getUniformLocation program name
    useProgram program
    GL.Uniform3(loc, value)

/// <summary>
/// Creates a shader with the provided specifiers.
/// Currently only creates a shader program with fragment and vertex shader.
/// </summary>
/// <param name="shaderSpecifiers">The first parameter contains the shader type, the second specifies the path to the shader source.</param>
let create shaderSpecifiers =
    let loadAndCompileShader (shaderType:ShaderType) path =
        let content = 
            match Utils.File.tryReadAllText path with 
            | Some a -> a 
            | _ -> failwith "Error while loading shader source"
        let shader = GL.CreateShader(shaderType)
        do GL.ShaderSource(shader, content)
        do GL.CompileShader shader
#if DEBUG
        let res = GL.GetShaderInfoLog shader
        printfn "%A" res
        printfn "%A" (GL.GetError())
#endif
        shader

    let shaders = 
        shaderSpecifiers 
        |> Array.map(fun (kind, path) -> loadAndCompileShader kind path)
    let program = GL.CreateProgram()
    for shader in shaders do GL.AttachShader(program, shader)
    GL.LinkProgram(program)
    for shader in shaders do GL.DeleteShader(shader)
    program |> ShaderProgram

