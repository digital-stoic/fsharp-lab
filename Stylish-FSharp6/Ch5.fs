module Ch5

open System.IO

let description = "Immutability and mutation"

let latestWriteTime (path: string) (searchPattern: string) =
    Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories)
    |> Seq.map File.GetLastWriteTime
    |> Seq.max

module Seq =
    let tryMax s =
        if s |> Seq.isEmpty then
            None
        else
            s |> Seq.max |> Some

let latestWriteTime_2 (path: string) (searchPattern: string) =
    Directory.EnumerateFiles(path, searchPattern, SearchOption.AllDirectories)
    |> Seq.map File.GetLastWriteTime
    |> Seq.tryMax

let tryGetSomethingFromApi =
    let mutable thingCount = 0
    let maxThings = 10

    fun () ->
        if thingCount < maxThings then
            thingCount <- thingCount + 1
            "Soup"
        else
            null

let rec apiToSeq () =
    seq {
        match tryGetSomethingFromApi () |> Option.ofObj with
        | Some thing ->
            yield thing
            yield! apiToSeq ()
        | None -> ()
    }

let listThingsFromApi () =
    apiToSeq () |> Seq.iter (printfn "I got %s")

let run =
    printfn $"Starting {description}..."

    latestWriteTime (Path.GetFullPath "./") ("*")
    |> printfn "%A"
    // latestWriteTime (Path.GetFullPath "./") ("NOT")
    // |> printfn "%A"
    latestWriteTime_2 (Path.GetFullPath "./") ("NOT")
    |> printfn "%A"

    listThingsFromApi ()
