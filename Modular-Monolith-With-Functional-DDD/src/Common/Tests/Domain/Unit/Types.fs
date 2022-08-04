namespace MyCompany.Meeting.Common.Domain.Test.Unit

open Expecto
open Expecto.Flip.Expect
open MyCompany.Meeting.Test.Common.Helper
open MyCompany.Meeting.Common.Domain.Test.Unit.Data

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

    let tests =
        testList "String50" [ H.testSimpleTypeOK "Create a String50 of length < 50" create value string50OK1 ]
