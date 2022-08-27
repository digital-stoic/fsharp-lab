open Expecto
open MyCompany.MyApp.Common.Test.Helper

[<Tests>]
let tests =
    testList "MyCompany.MyApp.Module.Module1.Test.Integration.Local"
    <| testListAppend [ MyCompany.MyApp.Module.Module1.Test.Integration.Local.tests ]

[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
