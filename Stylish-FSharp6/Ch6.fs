module Ch6

open System
open System.Text.RegularExpressions

let description = "Pattern matching"

// NOTE: Module with internal private type so that can't bypass validation at creation
module Heading =
    type Heading =
        private
        | Heading of double
        member this.Value = this |> fun (Heading h) -> h

    let create d = Heading d

// NOTE: Active patterns https://fsharpforfunandprofit.com/posts/convenience-active-patterns/
// NOTE: Active patterns with &
let regexZip = @"^\d{5}$"

let regexPostCode = @"^[A-Z](\d|[A-Z]){1,3} \d[A-Z]{2}$"

let (|PostalCode|) pattern s =
    let m = Regex.Match(s, pattern)
    if m.Success then Some s else None

let zipCodes = [ "90210"; "94043"; "10013"; "1OO13" ]

let postCodes =
    [ "SW1A 1AA"
      "GU9 0RA"
      "PO8 0AB"
      "P 0AB" ]

let run =
    printfn $"Starting {description}..."
    // (Heading 1.) |> printfn "%A"
    (Heading.create 1.) |> printfn "%A"

    zipCodes
    |> List.choose (fun (PostalCode regexZip p) -> p)
    |> printfn "%A"

    postCodes
    |> List.choose (fun (PostalCode regexPostCode p) -> p)
    |> printfn "%A"
