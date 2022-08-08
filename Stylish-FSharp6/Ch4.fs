module Ch4

let description = "Working effectively with collection functions"

type House = { Address: string; Price: decimal }

let houses_1 =
    [| { Address = "1 Acacia Avenue"
         Price = 250_000m }
       { Address = "2 Bradley Street"
         Price = 380_000m }
       { Address = "1 Carlton Road"
         Price = 98_000m } |]

let cheapHouses =
    houses_1
    |> Array.filter (fun h -> h.Price < 100_000m)

module House =
    let private random = System.Random(Seed = 42)

    let getRandom count =
        Array.init count (fun i ->
            { Address = sprintf "%i Stochastic Street" (i + 1)
              Price = random.Next(50_000, 500_000) |> decimal })

module Distance =
    let private random = System.Random(Seed = 42)

    let tryToSchool (house: House) =
        let dist = random.Next(10) |> double
        if dist < 5. then Some dist else None

type PriceBand =
    | Cheap
    | Medium
    | Expensive

module PriceBand =
    let fromPrice (price: decimal) =
        if price < 100_000m then Cheap
        elif price < 200_000m then Medium
        else Expensive

let houses = House.getRandom 20

// NOTE: Genericzero from https://fsharp.github.io/fsharp-core-docs/reference/fsharp-core-languageprimitives.html
// NOTE: Use inline functions for static type parameters https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/functions/inline-functions0
// NOTE: Better to define your own try... function
module Average =
    let inline averageOrZero (values: 'T []) =
        if values.Length = 0 then
            LanguagePrimitives.GenericZero<'T>
        else
            values |> Array.average

    let inline tryAverage (values: 'T []) =
        if values.Length = 0 then
            None
        else
            values |> Array.average |> Some

let run =
    printfn $"Starting {description}..."
    // cheapHouses |> printfn "%A"
    houses
    |> Array.map (fun h -> sprintf "Address: %s - Price: %f" h.Address h.Price)
    |> printf "%A\n"

    houses
    |> Array.averageBy (fun h -> h.Price)
    |> printfn "Average %f"

    houses
    |> Array.filter (fun h -> h.Price > 250_000m)
    |> printf "> 250,000 %A\n"

    houses
    |> Array.choose (fun h ->
        match Distance.tryToSchool h with
        | Some _a -> Some h
        | None -> None)
    |> printfn "Distance: %A"

    houses
    |> Array.groupBy (fun h -> PriceBand.fromPrice h.Price)
    |> printfn "%A"

    [| 10.; 100.; 1000. |]
    |> Average.averageOrZero
    |> printfn "%A"

    ([||]: float [])
    |> Average.averageOrZero
    |> printfn "%A"

    // [||]
    // |> Average.averageOrZero<float>
    // |> printfn "%A"

    houses
    |> Array.map (fun h -> h.Price)
    |> Array.filter (fun p -> p > 200_000m)
    |> Average.tryAverage
    |> printfn "%A"
