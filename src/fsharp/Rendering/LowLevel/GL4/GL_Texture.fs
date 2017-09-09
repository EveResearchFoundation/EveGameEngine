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

    let create (texture:Texture2D) =
        let tid = GL.GenTexture()
        let bufferId = GL.GenBuffer()

        GL.BindTexture(TextureTarget.Texture2D, tid)
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data)
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D)
        GL.BindTexture(TextureTarget.Texture2D, 0);
        { ID = tid }

