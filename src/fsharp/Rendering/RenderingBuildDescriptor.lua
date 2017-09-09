project ("Renderer")
    dotnetframework "4.6"
    kind "SharedLib"
    language "F#"

    files {
        "RendererUtils.fs"
    }
    
    -- Abstraction layer source files come first
    include("AbstractionLayer/SourceOrder.lua")

    files { 
        "RendererResourceManager.fs",
        "Scene.fs"
    }

    -- Low Level source files must be afterwards
    include("LowLevel/GL4/SourceOrder.lua")
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