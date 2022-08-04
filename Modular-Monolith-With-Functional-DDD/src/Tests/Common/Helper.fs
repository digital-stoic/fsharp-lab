module MyCompany.Meeting.Test.Common.Helper

open Expecto.Flip.Expect

// Append test lists
let testListAppend tests =
    List.fold (fun testList test -> List.append testList [ test ]) [] tests

// equal for Result
let equalR message expected actual =
    match actual with
    | Ok a -> equal message expected a
    | Error e -> isTrue "Force fail" false

// equal for constrained types
let equalRV typeValue message expected actual =
    match actual with
    | Ok a -> equal message expected (a |> typeValue)
    | Error _ -> isTrue "Force fail" false

// equal for optional constrained types
let equalRVO typeValue message expected actual =
    match actual with
    | Ok None -> equal message expected null
    | Ok (Some a) -> equal message expected (a |> typeValue)
    | Error _ -> isTrue "Force fail" false
