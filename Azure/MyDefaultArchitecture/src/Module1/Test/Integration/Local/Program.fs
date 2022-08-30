open System.Diagnostics
open Expecto
open MyCompany.MyApp.Common.Test.Helper
open MyCompany.MyApp.Module.Module1

let setup = Test.Integration.Local.asyncSetup
let waitSetupReady = Test.Integration.Local.asyncAwaitSetupReady

[<Tests>]
let tests =
    testList "MyCompany.MyApp.Module.Module1.Test.Integration.Local"
    <| testListAppend [ Test.Integration.Local.tests ]

let asyncTest (setup: Async<Process>) (waitSetupReady) argv =
    async {
        let! testHttpServer = setup
        do! waitSetupReady

        Tests.runTestsInAssemblyWithCLIArgs [] argv
        |> ignore

        testHttpServer.Kill()
    }

[<EntryPoint>]
let main argv =
    asyncTest setup waitSetupReady argv
    |> Async.RunSynchronously
    |> ignore

    0
