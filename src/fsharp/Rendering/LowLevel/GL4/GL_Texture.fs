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

type Texture2DAttachmentLL = {
    ID : int
}

module TextureAttachmentLL =
    
    open OpenTK.Graphics.OpenGL4
    
    open Renderer.AbstractionLayer
    open Texture
    open System.Collections.Generic
    
    let private existingTextures = Dictionary()

    let create (texture:Texture2D) =
        if existingTextures.ContainsKey texture.Guid |> not then
            let tid = GL.GenTexture()
            GL.BindTexture(TextureTarget.Texture2D, tid)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat |> int)  
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, OpenTK.Graphics.OpenGL4.TextureWrapMode.Repeat |> int)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, OpenTK.Graphics.OpenGL4.TextureMinFilter.Linear|> int)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, OpenTK.Graphics.OpenGL4.TextureMagFilter.Linear|> int)
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, texture.Data)
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D)
            GL.BindTexture(TextureTarget.Texture2D, 0);
            let res = { ID = tid }
            existingTextures.[texture.Guid] <- res
            res
        else
            existingTextures.[texture.Guid]