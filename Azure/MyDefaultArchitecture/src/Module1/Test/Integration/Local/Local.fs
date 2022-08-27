namespace MyCompany.MyApp.Module.Module1.Test.Integration

open Expecto
open Expecto.Flip.Expect

module Local =
    let tests = testList "Dummy" [ test "Dummy" { 42 |> equal "OK" 42 } ]
