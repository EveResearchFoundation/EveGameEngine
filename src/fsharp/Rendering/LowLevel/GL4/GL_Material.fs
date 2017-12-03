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

module Material =
    open Shader        
    open OpenTK.Graphics.OpenGL4

    let activate (shader:ShaderProgram) (material:Renderer.AbstractionLayer.Material.Material) =
        Shader.useProgram shader
        let materialColorsToSet = [| 
            "Material.ambient", material.Ambient
            "Material.diffuse", material.Diffuse
            "Material.specular", material.Specular
            "Material.emissive", material.Emissive |]
        
        let setColor (a, b) = Shader.setVec4 a b shader
        materialColorsToSet |> Array.iter setColor
        let activateTexture arg n =
            match arg with
            | Some (value:uint32) -> 
                let res = 
                    Renderer.Manager.TextureManager.getTextureWithId value 
                    |> Renderer.LowLevel.GL4.TextureAttachmentLL.create  
                GL.ActiveTexture n
                GL.BindTexture(TextureTarget.Texture2D, res.ID);
            | _ -> ()
        activateTexture material.DiffuseTexture TextureUnit.Texture0
        activateTexture material.SpecularTexture TextureUnit.Texture1
        activateTexture material.EmissiveTexture TextureUnit.Texture2
        Shader.setFloat32 "Material.shininess" material.Shininess shader 
        Shader.setVec3 "Material.diffuse" (Vec3 material.Diffuse) shader
        Shader.setVec3 "Material.ambient" (Vec3 material.Ambient) shader
    
    let deactivate () =
        GL.BindTexture(TextureTarget.Texture2D, 0)
