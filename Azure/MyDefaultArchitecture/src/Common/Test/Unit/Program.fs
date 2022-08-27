open Expecto
open MyCompany.MyApp.Common.Test.Helper

[<Tests>]
let tests =
    testList "MyCompany.MyApp.Common.Test.Unit"
    <| testListAppend [ MyCompany.MyApp.Common.Test.Unit.Answer.tests ]

[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
