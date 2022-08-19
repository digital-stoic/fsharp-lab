module MyCompany.MyApp.Deploy.Run

open System
open Argu
open MyCompany.MyApp.Deploy.Azure.Helper
open MyCompany.MyApp.Deploy.Azure.Resource


type DeployTargets =
    | Build
    | Test
    | Clean

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

let parse argv =
    let results = parser.ParseCommandLine argv

    match results.GetResult(Target) with
    | Build -> deployBuild resourceGroupName locationStr deployment
    | Test -> deployTest
    | Clean -> deployClean resourceGroupName
    |> Async.RunSynchronously
    |> ignore
