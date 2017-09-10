project ("Core")
    dotnetframework "4.6"
    kind "SharedLib"
    language "F#"

    files {
        "World.fs",
        -- "GameWindow.fs",
        "CoreBuildDescriptor.lua"
    }

    -- filter "system:windows"
    --     files { pathToAssimpDll }

    -- Linux and Mac OSX missing here...

    links {
        "System",
        "FSharp.Core",
        "Utils",
        "Renderer",
        pathToOpenTKDll
    }