namespace MyCompany.MyApp.Common.Test.Unit

open Expecto
open Expecto.Flip.Expect
// open MyCompany.MyApp.Common.Test.Helper

module Answer =
    open MyCompany.MyApp.Common.Domain.Answer

    let tests =
        testList
            "Answer"
            [ test "Answer is 42" { answer.value |> equal "Answer value is 42" 42 }
              test "Answer is not 43" {
                  answer.value
                  |> notEqual "Answer value is not 43" 43
              } ]
