[<AutoOpen>]
module RendererUtils
type Vec4 = OpenTK.Vector4
type Vec3 = OpenTK.Vector3
type Vec2 = OpenTK.Vector2
type Mat4 = OpenTK.Matrix4
type Mat3 = OpenTK.Matrix3

let inline vec3FromAssimpVec3 (vec3:Assimp.Vector3D) = Vec3(vec3.X, vec3.Y, vec3.Z)
let inline vec4FromAssimpVec4 (vec4:Assimp.Color4D) = Vec4(vec4.R, vec4.G, vec4.B, vec4.A)