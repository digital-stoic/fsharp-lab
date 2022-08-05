open Expecto
open MyCompany.Meeting.Test.Common.Helper

[<Tests>]
let tests =
    testList "Unit"
    <| testListAppend [ MyCompany.Meeting.Common.Domain.Test.Unit.String50.tests
                        MyCompany.Meeting.Common.Domain.Test.Unit.String200.tests ]

[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
