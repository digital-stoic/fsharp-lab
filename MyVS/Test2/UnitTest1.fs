module Test2

open NUnit.Framework
open Expecto
open Expecto.Flip.Expect

[<SetUp>]
let Setup () = ()

[<Test>]
let Test2 () = Assert.Pass()

[<Ignore("Not working with NUnit")>]
[<Test>]
let TestExpecto () =
    test "Test Expecto is OK" { 42 |> equal "42 always OK" 42 }
    |> ignore

[<Description("Answer is always 42!!!")>]
[<Test>]
let TestExpecto2 () = 42 |> equal "42 always ok" 42

[<Test>]
let TestExpecto3 () = 42 |> notEqual "43 is not ok" 43
