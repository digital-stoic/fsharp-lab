module Ch3

open System

let description = "Missing data"

type BillingDetails =
    { Name: string
      Billing: string
      Delivery: string option }

let myOrder =
    { Name = "Kit Eason"
      Billing = "112 Fibonacci street\n3583"
      Delivery = None }

let hisOrder =
    { Name = "John Doe"
      Billing = "314 Pi Avenue"
      Delivery = Some "16 Planck parkway\n62291" }

// NOTE: Option module https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-optionmodule.html
let addressForPackage (details: BillingDetails) =
    let address =
        details.Delivery
        |> Option.defaultValue details.Billing

    sprintf "%s\n%s" details.Name address

let printDeliveryAdress (details: BillingDetails) =
    details.Delivery
    |> Option.map (fun address -> address.ToUpper())
    |> Option.iter (fun address -> printfn "%s\n%s" (details.Name.ToUpper()) address)

let tryLastLine (address: string) =
    let parts = address.Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)

    match parts with
    | [||] -> None
    | parts -> parts |> Array.last |> Some

let tryPostalCode (codeString: string) =
    match Int32.TryParse codeString with
    | true, i -> Some i
    | false, i -> None

let postalCodeHub (code: int) = if code = 62291 then "Hub1" else "Hub2"

let tryHub (details: BillingDetails) =
    details.Delivery
    |> Option.bind tryLastLine
    |> Option.bind tryPostalCode
    |> Option.map postalCodeHub

type Delivery =
    | AsBilling
    | Physical of string
    | Download

type BillingDetails_2 =
    { Name: string
      Billing: string
      Delivery: Delivery }

let tryDeliveryLabel (details: BillingDetails_2) =
    match details.Delivery with
    | AsBilling -> details.Billing |> Some
    | Physical address -> address |> Some
    | Download -> None

let deliveryLabels (details: BillingDetails_2 seq) = details |> Seq.choose tryDeliveryLabel

let myOrder_2 =
    { Name = "Kit Eason"
      Billing = "112 Fibonacci street\n3583"
      Delivery = AsBilling }

let hisOrder_2 =
    { Name = "John Doe"
      Billing = "314 Pi Avenue"
      Delivery = Physical "16 Planck parkway\n62291" }

let herOrder =
    { Name = "Jane Smith"
      Billing = "9 Gravity road\n80885"
      Delivery = Download }

let run =
    printfn $"Starting {description}..."
    // printfn "%s" myOrder.Delivery
    printfn $"{myOrder.Delivery}"
    myOrder |> printDeliveryAdress
    hisOrder |> printDeliveryAdress
    myOrder |> tryHub |> printfn "%A"
    hisOrder |> tryHub |> printfn "%A"

    [ myOrder_2; hisOrder_2; herOrder ]
    |> deliveryLabels
    |> printfn "%A"
