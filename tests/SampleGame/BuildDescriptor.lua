supportFileName = "GeneratedSupportFile.fs"
copyrightNotice =
"// --------------------------------------------------------------------------\n" ..
"//  Copyright (c) 2017 Victor Peter Rouven Müller\n" ..
"//\n" ..
"//  Licensed under the Apache License, Version 2.0 (the \"License\");\n" ..
"//  you may not use this file except in compliance with the License.\n" ..
"//  You may obtain a copy of the License at\n" ..
"//\n" ..
"//      http://www.apache.org/licenses/LICENSE-2.0\n" ..
"//\n" ..
"//  Unless required by applicable law or agreed to in writing, software\n" ..
"//  distributed under the License is distributed on an \"AS IS\" BASIS,\n" ..
"//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.\n" ..
"//  See the License for the specific language governing permissions and\n" ..
"//  limitations under the License.\n" ..
"// --------------------------------------------------------------------------\n"

function generateSupportFile()
    nanosuitPath = pathToModelDir .. "/nanosuit/nanosuit.obj"
    brainStemPath = pathToModelDir .. "/BrainStem/BrainStem.gltf"
    supportFilePath = sampleGameDir .. "/" .. supportFileName
    
    content = copyrightNotice
    content = content .. "// !!- This file is generated, all changes will be lost after regeneration of project files! -!!//\n" 

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
    writeDefinitionLine("brainStemPath",  "\"".. brainStemPath .. "\"")
    
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
        "Core",
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
    
    
    