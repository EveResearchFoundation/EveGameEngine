
srcdir = path.getabsolute("./src");
fsharpdir = path.getabsolute("./src/fsharp")
cppdir = path.getabsolute("./src/cpp")

solution "EveGameEngine"
    configurations { "Debug", "Release" }
    platforms { "x64" }
    dotnetframework  "4.6"

    characterset "Unicode"
    
    include (fsharpdir)
        -- include (cppdir)
    