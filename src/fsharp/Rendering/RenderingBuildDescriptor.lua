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

    files {
        "RendererOptions.fs",
        "Renderer.fs",
        "RenderingBuildDescriptor.lua",
        pathToShadersDir .. "/*.*"
    }

    filter "system:windows"
        files { pathToAssimpDll }

    -- Linux and Mac OSX missing here...

    links {
        "System",
        "FSharp.Core",
        "Utils",
        "System.Drawing",
        pathToGlTFLoader2,
        pathToOpenTKDll,
        pathToManagedAssimpDll
    }

    filter "files:**.vert"
        buildaction "Embed"
    filter "files:**.frag"
        buildaction "Embed"