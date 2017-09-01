project "Utils"
    dotnetframework  "4.6"
    kind "SharedLib"
    language "F#"
    files 
    {   "Math/MathExtensions.fs",
        "Profiling/Profiling.fs" }
    
    links {
        "System",
        "FSharp.Core"
    }