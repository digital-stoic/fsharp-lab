module MyCompany.MyApp.Deploy.Azure.Helper

open System
open Farmer
open Farmer.Builders
open Fake.Core
open MyCompany.MyApp.Deploy.Console

let executeAzCli resourceGroupName command =
    async {
        Command.RawCommand("az", Arguments.OfArgs command)
        |> CreateProcess.fromCommand
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore
    }

let createResourceGroup resourceGroupName locationStr =
    executeAzCli
        resourceGroupName
        [ "group"
          "create"
          "--location"
          locationStr
          "--resource-group"
          resourceGroupName ]

let deleteResourceGroup resourceGroupName =
    executeAzCli
        resourceGroupName
        [ "group"
          "delete"
          "--name"
          resourceGroupName
          "--yes" ]

let executeDeployment resourceGroupName deployment =
    async {
        let deploymentResult =
            deployment
            |> Deploy.tryExecute resourceGroupName []

        match deploymentResult with
        | Ok outputs -> printfn "Success! Output: %A" outputs
        | Error error -> printfn "Failed! %s" error
    }

let deployBuild resourceGroupName locationStr deployment =
    Async.Sequential(
        seq {
            printStatus "Deploying build..."
            createResourceGroup resourceGroupName locationStr
            executeDeployment resourceGroupName deployment
        }
    )

let deployTest = Async.Sequential(seq { printError "Not implemented!" })

let deployClean resourceGroupName =
    Async.Sequential(
        seq {
            printStatus "Cleaning..."
            deleteResourceGroup resourceGroupName
        }
    )
