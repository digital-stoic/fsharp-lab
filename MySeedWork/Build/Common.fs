module Common

open Farmer
open Fake.Core
open Fake.IO

//==============================================================================
// Helpers
//==============================================================================

// let checkCommandExists cmd =
//     Command.RawCommand("which", Arguments.OfArgs([ cmd ]))

// let runCommand cmd =
//     cmd
//     |> CreateProcess.fromCommand
//     |> CreateProcess.ensureExitCode
//     |> Proc.run
//     |> ignore

let shellCommand executableOnPath args workingDir =
    let commandPath =
        ProcessUtils.tryFindFileOnPath executableOnPath
        |> Option.defaultWith (fun () -> failwith $"{executableOnPath} was not found in path")

    Command.RawCommand(commandPath, Arguments.OfArgs args)
    |> CreateProcess.fromCommand
    |> CreateProcess.withWorkingDirectory workingDir
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore
