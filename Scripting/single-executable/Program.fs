open Fake.Core

let result =
    Command.RawCommand("id", Arguments.OfArgs [])
    |> CreateProcess.fromCommand
    |> CreateProcess.redirectOutput
    |> Proc.run

[<EntryPoint>]
let main argv = 
    printfn "%A" result.Result.Output
    0
