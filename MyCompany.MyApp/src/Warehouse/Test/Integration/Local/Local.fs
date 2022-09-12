module MyCompany.MyApp.Warehouse.Test.Infrastructure.Integration.Local

open NUnit.Framework
open Expecto.Flip.Expect
open System.IO
open System.Diagnostics
open Hopac
open HttpFs.Client

let infrastructureAzureFolder =
    let curDir = Directory.GetCurrentDirectory()
    let upBuildDir = Path.Combine("..", "..", "..")
    let upDomainDir = Path.Combine("..", "..", "..", "Infrastructure", "Azure")
    Path.Combine(curDir, upBuildDir, upDomainDir)

let requestFunction1 =
    async {
        let request =
            Request.createUrl Get "http://localhost:7071/api/MyCompany-MyApp-Warehouse-Function1"
            |> Request.responseAsString

        let! response = request |> Hopac.startAsTask
        return response
    }

[<SetUp>]
let Setup () =
    let isSetupReady request =
        async {
            try
                let! _ = request
                return true
            with
            | _ -> return false
        }

    // TODO: with 'using'?
    let asyncSetup =
        async {
            let p = new Process()
            p.StartInfo.FileName <- "func"
            p.StartInfo.Arguments <- "host start"
            p.StartInfo.WorkingDirectory <- infrastructureAzureFolder
            p.StartInfo.UseShellExecute <- true
            p.Start() |> ignore
            return ()
        }

    let asyncAwaitSetupReady =
        let awaitLoop =
            async {
                let rec loop _ =
                    async {
                        do! Async.Sleep(1000)
                        let! isReady = isSetupReady requestFunction1

                        if (isReady) then
                            return ()
                        else
                            return! loop ()
                    }

                return! loop ()
            }

        async { do! awaitLoop }

    let asyncTest =
        async {
            let! testHttpServer = asyncSetup
            do! asyncAwaitSetupReady
        //testHttpServer.Kill()
        }

    asyncTest |> Async.RunSynchronously |> ignore

[<Test>]
let Test1 () =
    //System.Threading.Thread.Sleep(10000000)
    async {
        let! response = requestFunction1 |> Async.StartAsTask

        response
        |> equal "OK" """{"id":"42","type":{"case":"Barrel"},"size":195}"""
    //response
    //|> Application.Json.Answer.deserialize
    //|> Result.bind Application.Dto.Answer.toDomain
    //|> Result.defaultValue Domain.Answer.wrong
    //|> Domain.Answer.value
    //|> equal "Answer value is 42" 42
    }
