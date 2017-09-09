project ("Core")
    dotnetframework "4.6"
    kind "SharedLib"
    language "F#"

    packagesDir = "../../../packages/"

    files {
        "World.fs",
        "CoreBuildDescriptor.lua"
    }

    -- filter "system:windows"
    --     files { packagesDir .. "AssimpNet/build/native/win-x64/Assimp64.dll" }

    -- Linux and Mac OSX missing here...

    links {
        "System",
        "FSharp.Core",
        "Utils",
        "Renderer",
        packagesDir .. "OpenTK/lib/net20/OpenTK.dll"
    }