namespace MyCompany.MyOrder.Test.Unit

open NUnit.Framework
open Expecto.Flip.Expect

module Application =
    [<SetUp>]
    let Setup () = ()

    [<Test>]
    let ultimateAnswerIs42 () =
        let expected = 42
        let actual = 42
        expected |> equal "Answer value is 42" actual
