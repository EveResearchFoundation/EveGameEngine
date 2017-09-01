module Utils

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