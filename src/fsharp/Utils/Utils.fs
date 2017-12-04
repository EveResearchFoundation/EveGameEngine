module Utils

let (+/) path1 path2 = System.IO.Path.Combine(path1, path2)

[<Interface>]
type IResourceManaged =
    abstract ID : System.Guid

/// <summary>
/// Provides F# friendly functions from the System.Collections namespace
/// </summary>
[<AutoOpen>]
module Collections =
    
    /// <summary>
    /// Provides F# friendly functions from the System.Collections.Generic namespace
    /// </summary>
    [<AutoOpen>]
    module Generic =
        open System.Collections.Generic

        /// <summary>
        /// Creates a key value pair from an existing key and value
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value, which the key refers to</param>
        let inline createKeyValuePair key value = KeyValuePair(key, value)
    
module File =
    open System.IO
    open System

    let tryReadAllLines path =
        try
            File.ReadAllLines path |> Some
        with
        #if Debug
        | :? FileNotFoundException as ex -> 
            printfn "File not found"
            None
        | :? NotSupportedException as ex ->
            printfn "Path format is not supported"
            None
        | _ as ex ->
            printfn "%A" ex
            None
        #endif
        _ -> None;

    let tryReadAllText path =
        try
            File.ReadAllText path |> Some
        with
        #if Debug
        | :? FileNotFoundException as ex -> 
            printfn "File not found"
            None
        | :? NotSupportedException as ex ->
            printfn "Path format is not supported"
            None
        | _ as ex ->
            printfn "%A" ex
            None
        #endif
        _ -> None;

    let tryGetFileName path =
        try
            let fullPath = Path.GetFullPath path
            Path.GetFileName fullPath |> Some
        with
            #if Debug
            | :? FileNotFoundException as ex -> 
                printfn "File not found"
                None
            | :? NotSupportedException as ex ->
                printfn "Path format is not supported"
                None
            | _ as ex ->
                printfn "%A" ex
                None
            #endif
            _ -> None;
        
    let tryGetFullPath path =
        try
            Path.GetFullPath path |> Some
        with
            #if Debug
            | :? FileNotFoundException as ex -> 
                printfn "File not found"
                None
            | :? NotSupportedException as ex ->
                printfn "Path format is not supported"
                None
            | _ as ex ->
                printfn "%A" ex
                None
            #endif
            _ -> None;
    
    let tryGetPathRoot path =
        try
            Path.GetPathRoot path |> Some
        with
            #if Debug
            | :? FileNotFoundException as ex -> 
                printfn "File not found"
                None
            | :? NotSupportedException as ex ->
                printfn "Path format is not supported"
                None
            | _ as ex ->
                printfn "%A" ex
                None
            #endif
            _ -> None;

    let tryGetPathParent path =
        try
            let absPath = Path.GetFullPath path
            (Directory.GetParent absPath).FullName |> Some
        with
            #if Debug
            | :? FileNotFoundException as ex -> 
                printfn "File not found"
                None
            | :? NotSupportedException as ex ->
                printfn "Path format is not supported"
                None
            | _ as ex ->
                printfn "%A" ex
                None
            #endif
            _ -> None;

module Logging =
    open System

    // helper function to set the console collor and automatically set it back when disposed
    let inline private consoleColor (fc : ConsoleColor) = 
        let current = Console.ForegroundColor
        Console.ForegroundColor <- fc
        { new IDisposable with
              member x.Dispose() = Console.ForegroundColor <- current }

    // printf statements that allow user to specify output color
    let private cprintf color str = Printf.kprintf (fun s -> use c = consoleColor color in printf "%s" s) str
    let inline private cprintfn color str = Printf.kprintf (fun s -> use c = consoleColor color in printfn "%s" s) str
    let inline private log kind msg = printfn "[%s] %s: %A" (System.DateTime.Now.ToLongTimeString()) kind msg
    let inline clog color kind msg = cprintfn color "[%s] %s: %A" (System.DateTime.Now.ToLongTimeString()) kind msg
    

    let inline logInfo msg = log "INFO" msg
    let inline logError msg = clog ConsoleColor.Red "ERROR" msg
    let inline logWarning msg = clog ConsoleColor.Yellow "WARNING" msg
    let inline logDebug msg = clog ConsoleColor.Magenta "DEBUG" msg

module IDisposable =
    open System

    let dispose obj = (obj :> IDisposable).Dispose()

