module Ch11

open System
open Result

let description = "Railway oriented programming"

type ValidationError =
    | MustNotBeNull
    | MustNotBeEmpty
    | MustNotBeWhitespace
    | MustContainMixedCase
    | MustContainOne of chars: string
    | ErrorSaving of exn: Exception


let notEmpty (s: string) =
    if isNull (s) then
        Error MustNotBeNull
    elif String.IsNullOrEmpty(s) then
        Error MustNotBeEmpty
    elif String.IsNullOrWhiteSpace(s) then
        Error MustNotBeWhitespace
    else
        Ok s

let mixedCase (s: string) =
    let hasUpper = s |> Seq.exists (Char.IsUpper)
    let hasLower = s |> Seq.exists (Char.IsLower)

    if hasUpper && hasLower then
        Ok s
    else
        Error MustContainMixedCase

let containsAny (cs: string) (s: string) =
    if s.IndexOfAny(cs.ToCharArray()) > -1 then
        Ok s
    else
        Error(MustContainOne cs)

let tidy (s: string) = s.Trim()

let save (fail: bool) (s: string) =
    let dbSave s : unit =
        printfn $"Saving password: '{s}'"

        if fail then
            raise <| Exception "Dummy Exception"

    try
        dbSave s |> Ok
    with
    | e -> Error(ErrorSaving e)

let _validateAndSave (fail: bool) =
    notEmpty
    >> bind mixedCase
    >> bind (containsAny "-_!?")
    >> map tidy
    >> bind (save fail)

let validateAndSave = _validateAndSave false

let validateAndSave' = _validateAndSave true

let savePassword =
    let log m = printfn $"Logging error: {m}"

    validateAndSave
    >> mapError (function
        | MustNotBeNull
        | MustNotBeEmpty
        | MustNotBeWhitespace -> sprintf "Password must be entered"
        | MustContainMixedCase -> sprintf "Password must contain upper and lower case characters"
        | MustContainOne cs -> sprintf "Password must contain one of the following characters: %s" cs
        | ErrorSaving e ->
            log e.Message
            sprintf "Internal error saving the password: %s" e.Message)

let inputs =
    [ null
      ""
      "  "
      "abc"
      "Abc"
      "Abc!   " ]

let run =
    printfn $"{description}..."

    inputs
    |> List.iter (fun i -> i |> validateAndSave |> printfn "%A")

    "Abc!  " |> validateAndSave' |> printfn "%A"

    inputs
    |> List.iter (fun i -> i |> savePassword |> printfn "%A")
