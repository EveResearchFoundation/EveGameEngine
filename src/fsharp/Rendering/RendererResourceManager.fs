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
namespace Renderer

module Manager =
    open System
    open System.Collections.Generic
    open Renderer.AbstractionLayer

    open Texture
    open Mesh
    open Model
    open Material
    open System.IO

    let textureMap = Dictionary<TextureReferenceID, Texture2D>()
    // let private textureMapBackwards = Dictionary<Texture2D, TextureReferenceID>()

    let materialMap = Dictionary<MaterialReferenceID, Material>()
    let materialMapBackwards = Dictionary<Material, MaterialReferenceID>()

    let private assimpContext = new Assimp.AssimpContext()
    let private currentAssembly = System.Reflection.Assembly.GetExecutingAssembly()

    let registerTexture texture =
        let id = Guid.NewGuid()
        textureMap.Add (id, texture)
        id
    
    let registerMaterial material =
        if materialMapBackwards.ContainsKey material then 
            materialMapBackwards.[material]
        else 
            let id = Guid.NewGuid()
            materialMap.Add(id, material)
            materialMapBackwards.Add(material, id)
            id

    let inline getTexture id = textureMap.[id]
    let inline getMaterial id = materialMap.[id]

    let private loadAssimpTexture (texture:Assimp.EmbeddedTexture) =
        if texture.HasNonCompressedData then
            let data = 
                texture.NonCompressedData 
                |> Array.collect(fun texel -> [| uint8 texel.A; uint8 texel.B; uint8 texel.G; uint8 texel.R |])
            let res = {
                Width = texture.Width
                Height = texture.Height
                Data = data
            }
            registerTexture res
        else
            failwith "not implemented"

    let loadModel path =
        let scene = assimpContext.ImportFile path
        let assimpMeshes = scene.Meshes |> Seq.toArray
        let assimpTextures = scene.Textures |> Seq.toArray
        let materialDict =
            let dict = Dictionary()
            let materialFromAssimpMaterial (material:Assimp.Material) =
                let res : Material = 
                    {   Ambient = material.ColorAmbient |> RendererUtils.vec4FromAssimpVec4
                        Diffuse = material.ColorDiffuse |> RendererUtils.vec4FromAssimpVec4
                        Specular = material.ColorSpecular |> RendererUtils.vec4FromAssimpVec4
                        Emissive = material.ColorEmissive |> RendererUtils.vec4FromAssimpVec4
                        Shininess = if material.Shininess <= 0.f then 1.f else material.Shininess }
                registerMaterial res

            scene.Materials
            |> Seq.toArray
            |> Array.mapi(fun i m -> i, materialFromAssimpMaterial m)
            |> Array.iter dict.Add
            dict

        let textureDict = 
            let dict = Dictionary()
            assimpTextures
            |> Array.mapi (fun i t -> i, loadAssimpTexture t)
            |> Array.iter (fun v -> dict.Add v)
            dict

        let loadMesh (assimpMesh:Assimp.Mesh) =
            let textureIDs = [| |]
                //assimpMesh.TextureCoordinateChannels
                //|> Array.mapi (fun i texCoords -> textureDict.[i])
            let convertAssimpVectorList = Seq.map RendererUtils.vec3FromAssimpVec3 >> Seq.toArray
            let vertices = assimpMesh.Vertices |> convertAssimpVectorList
            let normals = assimpMesh.Normals |> convertAssimpVectorList
            let vertices =
                [| for i in 0 .. (vertices.Length - 1) -> 
                    {   Position = vertices.[i]
                        Normal = normals.[i]
                        TexCoord = Vec2()       }  |]

            let materialID = 
                let index = assimpMesh.MaterialIndex
                materialDict.[index]
                
            {   Vertices = vertices
                Indices = assimpMesh.GetIndices()
                TextureReference = textureIDs 
                MaterialReference = materialID }

        let meshes = assimpMeshes |> Array.map loadMesh

        {   Meshes = meshes
            Translation = Mat4() }
        

    let tryGetContentOfEmbeddedTextFile name =
        try
            use stream = currentAssembly.GetManifestResourceStream name
            use reader = new StreamReader(stream)
            reader.ReadToEnd () |> Result.Ok 
        with e ->
            e |> Result.Error 
