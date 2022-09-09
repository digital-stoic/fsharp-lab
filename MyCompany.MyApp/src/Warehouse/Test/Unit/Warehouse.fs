module MyCompany.MyApp.Warehouse.Test.Unit

open NUnit.Framework
open Expecto.Flip.Expect
open MyCompany.MyApp.Warehouse.Domain

[<SetUp>]
let Setup () = ()

//[<Description("Description!!!")>]
//[<Category("Category!!!")>]
//[<Test>]
//let Test1 () = Assert.Pass()

[<Test>]
// TODO: With params and data?
let QuarterCaskSizeRangeIsCorrect () =
    let expected = (45, 50)

    CaskType.sizeRange QuarterCask
    |> equal $"{expected}" expected

[<Test>]
let CaskTypeSizeIsAverageSizeRange () =
    let (min, max) = CaskType.sizeRange QuarterCask
    let expected = (min + max) / 2

    CaskType.size QuarterCask
    |> equal $"{expected}" expected
