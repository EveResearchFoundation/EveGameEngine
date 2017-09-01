module Vertex
open System.Runtime.InteropServices
open OpenTK.Graphics.OpenGL4

#nowarn "9"
/// <summary>
/// Standard vertex definition for the EveGameEngine.
/// </summary>
[<Struct>]
[<StructLayout(LayoutKind.Sequential)>]
type Vertex = {
    Position:Vec3
    Normal:Vec3
    TexCoord:Vec2
}

/// <summary>
/// Helper function for retrieving the offset of a specific vertex attribute (struct).
/// </summary>
/// <param name="name"></param>
let offsetof name = Marshal.OffsetOf(typeof<Vertex>, name)

/// <summary>
/// Helper value indicating the offset of the `Position` field.
/// </summary>
let offset_Position = offsetof "Position@"

/// <summary>
/// Helper value indicating the offset of the `Normal` field.
/// </summary>
let offset_Normal = offsetof "Normal@"

/// <summary>
/// Helper value indicating the offset of the `TexCoord` field.
/// </summary>
let offset_TexCoord = offsetof "TexCoord@"

/// <summary>
/// Helper value indicating the size of the `Vertex` struct record.
/// </summary>
let size = Marshal.SizeOf<Vertex>()