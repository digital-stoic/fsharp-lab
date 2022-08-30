namespace MyCompany.MyApp.Common.Domain

type Answer = private Answer of int

module Answer =
    let create answer =
        match answer with
        | 42 -> 42 |> Answer |> Ok
        | _ ->
            let msg = "[Answer.create] There is only one possible answer"
            Error msg

    let value (Answer answer) = answer

    let wrong = 0 |> Answer
