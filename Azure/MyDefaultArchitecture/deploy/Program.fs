open System
open Argu

type DeployTargets =
    | Build
    | Test
    | Clean
    | NoTarget

type DeployArguments =
    | [<MainCommand>] Target of DeployTargets
    interface IArgParserTemplate with
        member arg.Usage =
            match arg with
            | Target _ -> "target [build|test|clean]"

let errorHandler =
    ProcessExiter(
        colorizer =
            function
            | ErrorCode.HelpText -> None
            | _ -> Some ConsoleColor.Red
    )

let parser =
    ArgumentParser.Create<DeployArguments>(programName = "deploy", errorHandler = errorHandler)

[<EntryPoint>]
let main argv =
    let results = parser.ParseCommandLine argv

    match results.GetResult(Target, defaultValue = NoTarget) with
    | Build -> printfn "build"
    | Test -> printfn "test"
    | Clean -> printfn "clean"
    | NoTarget -> printfn "none"

    0
