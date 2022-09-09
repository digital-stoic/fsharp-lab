module Test1

open NUnit.Framework

[<SetUp>]
let Setup () = ()

[<Test>]
let Test1 () =
    let x = 43
    Assert.Pass()
