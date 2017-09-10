
srcDir = path.getabsolute("./src");
fsharpDir = path.getabsolute("./src/fsharp")
cppDir = path.getabsolute("./src/cpp")
testsDir = path.getabsolute("./tests")
packagesDir = path.getabsolute("./packages")
sampleGameDir = path.getabsolute("./tests/SampleGame")

-- Dependency path defines
pathToAssimpDll = path.getabsolute(packagesDir .. "/AssimpNet/build/native/win-x64/Assimp64.dll")
pathToManagedAssimpDll = path.getabsolute(packagesDir .. "/AssimpNet/lib/net45/AssimpNet.dll")
pathToOpenTKDll = path.getabsolute(packagesDir .. "/OpenTK/lib/net20/OpenTK.dll")

-- Resource defines
pathToModelDir = path.getabsolute("./resources/models")
pathToShadersDir = path.getabsolute("./resources/shaders")

solution "EveGameEngine"
    configurations { "Debug", "Release" }
    platforms { "x64" }
    dotnetframework  "4.6"

    filter "configurations:Debug"
        symbols "On"
        defines { "DEBUG" }

    filter "configurations:Release"
        symbols "Off"
        optimize "On"

    characterset "Unicode"
    
    include (fsharpDir)
    include (testsDir)
        -- include (cppDir)
    