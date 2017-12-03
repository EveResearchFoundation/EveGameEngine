open System
#I @"../packages/FAKE/tools/"
#r @"FakeLib.dll"

open Fake
open Fake.FileHelper
open Fake.FileUtils

let configuration = getBuildParamOrDefault "configuration" "Release"
let action =
    let action = getBuildParamOrDefault "action" "vs2017"
    match action with 
    | "vs2015" | "vs2017" -> action 
    | _ -> failwith "unsupported action, only solution generation is allowed!"

module Premake =
    open System.IO
    let private os = 
        if isWindows then "windows"
        elif isMacOS then "osx"
        elif isLinux then "linux"
        else failwithf "Unsupported operating system!"
    
    let private premakePath, premakeExe =
        if isWindows then
            "premake5.exe"
        elif isMacOS then
            "premake5"
        else
            "premake5"
        |> (fun p -> p |> Path.GetFullPath |> Path.GetDirectoryName, p)
        

    let invoke action file =
        logfn "premake dir %s" premakePath
        ExecProcess(
            fun options ->
            options.WorkingDirectory <- premakePath
            options.Arguments <- sprintf "--file=%s %s" file action
            options.FileName <- premakeExe
        ) TimeSpan.MaxValue

let srcDir = "../src"

let fsharpDir = "fsharp"

let rendererDir = fsharpDir @@ "Renderer"

let utilsDir = fsharpDir @@ "Utils"

Target "Generate" (fun _ ->
    Premake.invoke action "\"premake5.lua\""
    |> logfn "Executed premake, result %d"
)

Target "Build-Utils" (fun _ ->
    !! (utilsDir + "**/*.fsproj")
    |> MSBuildWithDefaults ""
    |> Log "Build Utils"
)

Target "Build-Renderer" (fun _ ->
    !! ("src" @@ "Renderer" @@ "/**/*.fsproj")
    |> MSBuildWithDefaults ""
    |> Log "Build Utils"
)

Target "Build-All" ignore
Target "Default" ignore


"Build-All"
    ==> "Build-Renderer"

"Build-Renderer"
    ==> "Build-Utils"

"Build-Utils"
    ==> "Generate"

RunTargetOrDefault "Generate"