namespace MyCompany.MyApp.Common.Test.Unit

open FsToolkit.ErrorHandling
open Expecto
open Expecto.Flip.Expect
open MyCompany.MyApp.Common.Test.Helper

module Answer =
    open MyCompany.MyApp.Common.Domain

    let tests =
        testList
            "Answer"
            [ test "Answer is 42" {
                  Answer.create 42
                  |> Result.defaultValue Answer.wrong
                  |> Answer.value
                  |> equal "Answer value is 42" 42
              }
              test "Answer is not 43" {
                  Answer.create 43
                  |> notEqualR "Answer value is not 43" "[Answer.create] There is only one possible answer"
              } ]
