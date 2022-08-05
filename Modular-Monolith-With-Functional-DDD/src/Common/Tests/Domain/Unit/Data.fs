namespace MyCompany.Meeting.Common.Domain.Test.Unit.Data

module String50 =
    let OK1 = "This string is < 50"
    let longerNOK1 = String.init 51 (fun _ -> "x")

module String200 =
    let OK1 = "This string is < 200"
    let longerNOK1 = String.init 201 (fun _ -> "x")
