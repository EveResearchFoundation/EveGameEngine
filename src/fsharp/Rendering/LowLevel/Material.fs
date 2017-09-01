module Material
open Texture

[<Struct>]
type Material = {
    ColorAmbient : Vec3
    ColorDiffuse : Vec3
    ColorSpecular : Vec3
    ColorEmissive : Vec3
    TexturesAmbient : Texture[]
    Textures : System.IO.Path []
}