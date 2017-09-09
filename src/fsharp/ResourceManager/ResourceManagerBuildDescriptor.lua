project "ResourceManager"
    dotnetframework  "4.6"
    kind "SharedLib"
    language "F#"
    files {   
        "ResourceManager.fs",
        "ResourceManagerBuildDescriptor.lua"
    }
    links {
        "System",
        "FSharp.Core",
        "Utils"
    }