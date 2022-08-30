namespace MyCompany.MyApp.Module.Module1.Test.Integration

open Expecto
open Expecto.Flip.Expect
open Hopac
open HttpFs.Client
open System.Diagnostics

module Local =
    let infrastructureAzureFolder = "../../../Infrastructure/Azure"

    let requestFunction1 =
        async {
            let request =
                Request.createUrl Get "http://localhost:7071/api/MyCompany-MyApp-Module1-Function1"
                |> Request.responseAsString

            let! response = request |> Hopac.startAsTask
            return response
        }

    let isSetupReady request =
        async {
            try
                let! _ = request
                return true
            with
            | _ -> return false
        }

    let asyncSetup =
        async {
            let p = new Process()
            p.StartInfo.FileName <- "func"
            p.StartInfo.Arguments <- "host start"
            p.StartInfo.WorkingDirectory <- infrastructureAzureFolder
            p.StartInfo.UseShellExecute <- false
            p.Start() |> ignore
            return p
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

    let tests =
        testList
            "Function1"
            [ testTask "Get value 42" {
                  let! response = requestFunction1 |> Async.StartAsTask
                  response |> equal "OK" """{"value":42}"""
              } ]
