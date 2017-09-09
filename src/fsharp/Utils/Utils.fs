module Utils


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