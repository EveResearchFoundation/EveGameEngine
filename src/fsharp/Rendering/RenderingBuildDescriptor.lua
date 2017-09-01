project ("Renderer")
    dotnetframework "4.6"
    kind "SharedLib"
    language "F#"

    files "RendererUtils.fs"

    -- Low Level source files must be first
    include("LowLevel/SourceOrder.lua")
    -- Now we can continue with the GUI
    include("GUI/SourceOrder.lua")

    packagesDir = "../../../packages/"

    files {
        "RendererOptions.fs",
        "Renderer.fs",
        "RenderingBuildDescriptor.lua"
    }

    filter "system:windows"
        files { packagesDir .. "AssimpNet/build/native/win-x64/Assimp64.dll" }

    -- Linux and Mac OSX missing here...

    links {
        "System",
        "FSharp.Core",
        "Utils",
        packagesDir .. "OpenTK/lib/net20/OpenTK.dll",
        packagesDir .. "AssimpNet/lib/net45/AssimpNet.dll"
    }