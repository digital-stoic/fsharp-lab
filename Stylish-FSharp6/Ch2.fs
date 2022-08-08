module Ch2

open System

let description = "Designing functions using types"

// NOTE: Sketch function signature first wit NotImplemented exception
let convertMilesYards_1 (milesPointYards: float) : float = raise <| NotImplementedException()

// NOTE: Make illegal states unrepresentable
// NOTE: Number format "10_123."
type MileYards_1 = MileYards_1 of wholeMiles: int * yards: int

let create (milesPointYards: float) =
    let wholeMiles = milesPointYards |> floor |> int
    let fraction = milesPointYards - float (wholeMiles)

    if fraction > 0.1759 then
        raise
        <| ArgumentOutOfRangeException(nameof (milesPointYards), "Fractional part must be <= 0.1759")

    let yards = fraction * 10_000. |> round |> int
    MileYards_1(wholeMiles, yards)

let mileYardsToDecimalYards (mileYards: MileYards_1) : float =
    match mileYards with
    | (MileYards_1 (wholeMiles, yards)) -> (float wholeMiles) + ((float yards) / 1760.)

// NOTE: Using a module to associate functions with a type
// NOTE: Private type constructor
// NOTE: Pattern matching in parameter declarations
// NOTE: Use private operators to make the code less wordy. See https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/operator-overloading0
module MileYards =
    type MileYards = private MileYards of wholeMiles: int * yards: int

    let fromMileYardsToDecimalYards (milesPointYards: float) =
        let wholeMiles = milesPointYards |> floor |> int
        let fraction = milesPointYards - float (wholeMiles)

        if fraction > 0.1759 then
            raise
            <| ArgumentOutOfRangeException(nameof (milesPointYards), "Fractional part must be <= 0.1759")

        let yards = fraction * 10_000. |> round |> int
        MileYards(wholeMiles, yards)

    let toDecimalYards_1 (mileYards: MileYards) =
        match mileYards with
        | (MileYards (wholeMiles, yards)) -> (float wholeMiles) + ((float yards) / 1760.)

    let toDecimalYards_2 (MileYards (wholeMiles, yards)) =
        (float wholeMiles) + ((float yards) / 1760.)

    let toDecimalYards (MileYards (wholeMiles, yards)) =
        let (~~) = float
        ~~wholeMiles + (~~yards / 1760.)

let run =
    printfn $"Starting {description}..."
    // convertMilesYards_1 1.0 |> ignore
    // printfn $"{create 1.8}"
    printfn $"{create 1.0}"
    printfn $"{MileYards.fromMileYardsToDecimalYards 1.711}"
