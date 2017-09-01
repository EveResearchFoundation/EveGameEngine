project ("Renderer")
    dotnetframework "4.6"
    kind "SharedLib"
    language "F#"

    -- Low Level source files must be first
    include("LowLevel/SourceOrder.lua")
    -- Now we can continue with the GUI
    include("GUI/SourceOrder.lua")

    files {
        "RendererOptions.fs",
        "Renderer.fs"
    }

    links {
        "System",
        "FSharp.Core",
        "Utils"
    }