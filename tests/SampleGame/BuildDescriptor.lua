supportFileName = "GeneratedSupportFile.fs"

function generateSupportFile()
    nanosuitPath = pathToModelDir .. "/nanosuit/nanosuit.obj"
    supportFilePath = sampleGameDir .. "/" .. supportFileName
    
    content = "/// This file is generated, all changes will be lost after regeneration of project files!\n"
    depth = 0
    function indent ()
        for i = 1,depth do
            content = content .. "    "
        end
    end
    
    function newLine ()
        content = content .. "\n"
    end
    
    function appendLine (value) 
        indent()
        content = content .. value
        newLine()
    end
    
    function writeDefinitionLine (varName, value)
        line = "let " .. varName .. " = " .. value
        appendLine(line)
    end
    
    newLine()
    appendLine("module Support")
    -- depth = depth + 1
    writeDefinitionLine("nanosuitPath",  "\"".. nanosuitPath .. "\"")
    
    print("Content of generated code: \n" .. content .. "\n")
    
    print("------------\n")
    print("pathToGeneratedFile: " .. supportFilePath )
    os.remove(supportFilePath)
    io.writefile(supportFilePath , content)
end

project ("SampleGame")
    dotnetframework "4.6"
    kind "ConsoleApp"
    language "F#"
    
    generateSupportFile()
    
    files {
        supportFileName,
        "Program.fs",
        "BuildDescriptor.lua"
    }

    filter "system:windows"
        files {
            pathToAssimpDll
        }
        
    
    print("Packages dir: " .. packagesDir)
    -- Linux and Mac OSX missing here...
    print("assimp dll: " .. pathToAssimpDll)
    postbuildcommands {
        "{COPY}  " .. pathToAssimpDll .. " $(TargetDir)"
    }
    links {
        "System",
        "FSharp.Core",
        "Utils",
        "System.Drawing",
        "Renderer",
        pathToOpenTKDll,
        pathToManagedAssimpDll,
        pathToAssimpDll
    }

    filter "files:Assimp64.dll"
        buildaction "Copy"
    
    defines {
        "MODEL_NANOSUIT_OBJ_FILE=" .. nanosuitPath
    }
    
    -- premake.outln("<Import Project=\"../packages/AssimpNet.3.3.2/build/AssimpNet.targets\" Condition=\"Exists('../packages/AssimpNet.3.3.2/build/AssimpNet.targets')\" />")
    
    
    