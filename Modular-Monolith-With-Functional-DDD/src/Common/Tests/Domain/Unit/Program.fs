open Expecto
open MyCompany.Meeting.Test.Common.Helper
// open MyCompany.Meeting.Common.Domain.Test.Unit.String50

[<Tests>]
let tests =
    testList "Unit"
    <| testListAppend [ MyCompany.Meeting.Common.Domain.Test.Unit.String50.tests ]

[<EntryPoint>]
let main argv =
    Tests.runTestsInAssemblyWithCLIArgs [] argv
