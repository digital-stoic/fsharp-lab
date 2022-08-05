namespace MyCompany.Meeting.Common.Domain.Test.Unit

open Expecto
open Expecto.Flip.Expect
open MyCompany.Meeting.Test.Common.Helper

//==============================================================================
// Helpers
//==============================================================================
module H =
    let _testSimpleTypeOK equals testName create value x =
        test testName {
            create "test" x
            |> equals value "Simple type created" x
        }

    let testSimpleTypeNOK testName create x =
        test testName {
            create "test" x
            |> isError "Simple type creation failed"
        }

    let testSimpleTypeOK testName create value x =
        _testSimpleTypeOK equalRV testName create value x

    let testSimpleTypeOptionOK testName create x =
        _testSimpleTypeOK equalRVO testName create x

//==============================================================================
// Tests
//==============================================================================
module String50 =
    open MyCompany.Meeting.Common.Domain.String50
    open MyCompany.Meeting.Common.Domain.Test.Unit.Data.String50

    let tests =
        testList
            "String50"
            [ H.testSimpleTypeOK "Create a String50 of length < 50" create value OK1
              H.testSimpleTypeNOK "Fail to create a String50 of length > 50" create longerNOK1
              H.testSimpleTypeNOK "Fail to create a null String50" create null
              H.testSimpleTypeOptionOK "Create an optional String50 of length < 50" createOption value OK1
              H.testSimpleTypeOptionOK "Create a null optional String50" createOption value null
              H.testSimpleTypeNOK "Fail to create an optional String50 of length > 50" createOption longerNOK1 ]

module String200 =
    open MyCompany.Meeting.Common.Domain.String200
    open MyCompany.Meeting.Common.Domain.Test.Unit.Data.String200

    let tests =
        testList
            "String200"
            [ H.testSimpleTypeOK "Create a String200 of length < 200" create value OK1
              H.testSimpleTypeNOK "Fail to create a String200 of length > 200" create longerNOK1
              H.testSimpleTypeNOK "Fail to create a null String200" create null
              H.testSimpleTypeOptionOK "Create an optional String200 of length < 200" createOption value OK1
              H.testSimpleTypeOptionOK "Create a null optional String200" createOption value null
              H.testSimpleTypeNOK "Fail to create an optional String200 of length > 200" createOption longerNOK1 ]
