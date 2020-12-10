#r "nuget: Fake.Core.Target"

open System
open Fake.Core

let result =
    Command.RawCommand("id", Arguments.OfArgs [])
    |> CreateProcess.fromCommand
    |> CreateProcess.redirectOutput
    |> Proc.run

printfn "%A" result.Result.Output
