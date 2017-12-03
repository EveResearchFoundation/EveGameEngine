// --------------------------------------------------------------------------
//  Copyright (c) 2017 Victor Peter Rouven M�ller
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
    open System.IO

    open Renderer.AbstractionLayer

    open Texture
    open Mesh
    open Model
    open Material
    open Utils

    module TextureManager =
        open System.Drawing
        open System.Drawing.Imaging

        let mutable private counter = 0u;
        let private textureMap = Dictionary<string, uint32>()
        let private fastTextureMap = Dictionary<uint32, Texture2D>()

        let loadAndRegisterTexture (pathToTextureFile) =
            let textureFileName = 
                match File.tryGetFullPath pathToTextureFile with
                | Some filename -> filename
                | None -> failwithf "Path %A to texture is invalid!" pathToTextureFile
            if textureMap.ContainsKey textureFileName then textureMap.[textureFileName]
            else
                printfn "%A" pathToTextureFile
                use bitmap = Bitmap pathToTextureFile
                let data =
                    let ptr = 
                        bitmap.LockBits(Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
                        |> (fun a -> a.Scan0) 
                        |> NativeInterop.NativePtr.ofNativeInt<byte>
                    Array.Parallel.init (bitmap.Width * bitmap.Height * 4) (fun i -> NativeInterop.NativePtr.add ptr i |> NativeInterop.NativePtr.read)
                    //|> Array.chunkBySize 3
                    //|> Array.Parallel.collect Array.rev
                let rawTexture = 
                    {   Width = bitmap.Width
                        Height = bitmap.Height
                        Data = data 
                        Guid = Guid.NewGuid() }
                let id = counter
                textureMap.Add(textureFileName, id)
                fastTextureMap.Add(id, rawTexture)
                counter <- counter + 1u
                id
        
        let getTextureWithId id = fastTextureMap.[id]

    // let private textureMapBackwards = Dictionary<Texture2D, TextureReferenceID>()

    let materialMap = Dictionary<Guid, Material>()
    let materialMapBackwards = Dictionary<Material, Guid>()

    let private assimpContext = new Assimp.AssimpContext()
    let private currentAssembly = System.Reflection.Assembly.GetExecutingAssembly()
    
    let registerMaterial material =
        if materialMapBackwards.ContainsKey material then 
            materialMapBackwards.[material]
        else 
            let id = Guid.NewGuid()
            materialMap.Add(id, material)
            materialMapBackwards.Add(material, id)
            id

    let inline getMaterial id = materialMap.[id]

    // let ll path =
    //     let absPath = 
    //         match File.tryGetFullPath path with
    //         | Some p -> p
    //         | None -> failwith "Invalid model path"
    //     let model = glTFLoader.Interface.LoadModel path
    //     model.Textures
    //     model.Materials
    //     model.Skins
    //     model.Scene
    //     model.Scenes
    //     model.Textures.

    //     let materialDict =
    //         let dict = Dictionary()
    //         let materialFromAssimpMaterial (material:glTFLoader.Schema.Material) =
    //             let loadTexture (fileName : string) =
    //                 TextureManager.loadAndRegisterTexture (absRootDir +/ fileName)
    //             let ambientTexture = 
    //                 if material. then
    //                     loadTexture material.TextureAmbient.FilePath |> Some
    //                 else None
    //             let diffuseTexture = 
    //                 if material.HasTextureDiffuse then
    //                     loadTexture material.TextureDiffuse.FilePath |> Some
    //                 else None
    //             let specularTexture = 
    //                 if material.HasTextureSpecular then
    //                     loadTexture material.TextureSpecular.FilePath |> Some
    //                 else None
    //             let emissiveTexture = 
    //                 if material.HasTextureEmissive then
    //                     loadTexture material.TextureEmissive.FilePath |> Some
    //                 else None
    //             let res : Material = 
    //                 {   Ambient = material.ColorAmbient |> RendererUtils.vec4FromAssimpVec4
    //                     Diffuse = material.ColorDiffuse |> RendererUtils.vec4FromAssimpVec4
    //                     Specular = material.ColorSpecular |> RendererUtils.vec4FromAssimpVec4
    //                     Emissive = material.ColorEmissive |> RendererUtils.vec4FromAssimpVec4
    //                     Shininess = if material.Shininess <= 0.f then 1.f else material.Shininess 
    //                     AmbientTexture = ambientTexture 
    //                     DiffuseTexture = diffuseTexture 
    //                     SpecularTexture = specularTexture 
    //                     EmissiveTexture = emissiveTexture }
    //             registerMaterial res

    //         scene.Materials
    //         |> Seq.toArray
    //         |> Array.mapi(fun i m -> i, materialFromAssimpMaterial m)
    //         |> Array.iter dict.Add
    //         dict

    //     let loadMesh (assimpMesh:Assimp.Mesh) =
    //         let textureIDs =
    //             [| |]
    //         let convertAssimpVectorList = Seq.map RendererUtils.vec3FromAssimpVec3 >> Seq.toArray
    //         let vertices = assimpMesh.Vertices |> convertAssimpVectorList
    //         let normals = assimpMesh.Normals |> convertAssimpVectorList
    //         let vertices =
    //             [| for i in 0 .. (vertices.Length - 1) -> 
    //                 {   Position = vertices.[i]
    //                     Normal = normals.[i]
    //                     TexCoord = Vec2()       }  |]

    //         let materialID = 
    //             let index = assimpMesh.MaterialIndex
    //             materialDict.[index]
                
    //         {   Vertices = vertices
    //             Indices = assimpMesh.GetIndices()
    //             TextureReference = textureIDs 
    //             MaterialReference = materialID }

    //     let meshes = assimpMeshes |> Array.map loadMesh

    //     {   Meshes = meshes
    //         Translation = Mat4() }


    //     ()

    let loadModel path =
        let absRootDir = 
            match File.tryGetPathParent path with
            | Some p -> p 
            | None -> failwith "Modelpath is invalid"
        let scene = assimpContext.ImportFile(path, Assimp.PostProcessSteps.FlipUVs)
        let assimpMeshes = scene.Meshes |> Seq.toArray
        let assimpTextures = scene.Textures |> Seq.toArray
        
        let materialDict =
            let dict = Dictionary()
            let materialFromAssimpMaterial (material:Assimp.Material) =
                let loadTexture (fileName : string) =
                    TextureManager.loadAndRegisterTexture (absRootDir +/ fileName)
                let ambientTexture = 
                    if material.HasTextureAmbient then
                        loadTexture material.TextureAmbient.FilePath |> Some
                    else None
                let diffuseTexture = 
                    if material.HasTextureDiffuse then
                        loadTexture material.TextureDiffuse.FilePath |> Some
                    else None
                let specularTexture = 
                    if material.HasTextureSpecular then
                        loadTexture material.TextureSpecular.FilePath |> Some
                    else None
                let emissiveTexture = 
                    if material.HasTextureEmissive then
                        loadTexture material.TextureEmissive.FilePath |> Some
                    else None
                let res : Material = 
                    {   Ambient = material.ColorAmbient |> RendererUtils.vec4FromAssimpVec4
                        Diffuse = material.ColorDiffuse |> RendererUtils.vec4FromAssimpVec4
                        Specular = material.ColorSpecular |> RendererUtils.vec4FromAssimpVec4
                        Emissive = material.ColorEmissive |> RendererUtils.vec4FromAssimpVec4
                        Shininess = if material.Shininess = 0.f then 1.f else material.Shininess 
                        AmbientTexture = ambientTexture 
                        DiffuseTexture = diffuseTexture 
                        SpecularTexture = specularTexture 
                        EmissiveTexture = emissiveTexture }
                registerMaterial res

            scene.Materials
            |> Seq.toArray
            |> Array.mapi(fun i m -> i, materialFromAssimpMaterial m)
            |> Array.iter dict.Add
            dict

        let loadMesh (assimpMesh:Assimp.Mesh) =
            let textureIDs =
                [| |]
            let convertAssimpVectorList = Seq.map RendererUtils.vec3FromAssimpVec3 >> Seq.toArray
            let vertices = assimpMesh.Vertices |> convertAssimpVectorList
            let normals = assimpMesh.Normals |> convertAssimpVectorList
            let getTexCoord = 
                if assimpMesh.HasTextureCoords(0) then
                    (fun i -> assimpMesh.TextureCoordinateChannels.[0].[i] |> (fun v -> Vec2(v.X, v.Y)))
                else (fun _ -> Vec2())
            let vertices =
                [| for i in 0 .. (vertices.Length - 1) -> 
                    {   Position = vertices.[i]
                        Normal = normals.[i]
                        TexCoord = getTexCoord i } |]

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
