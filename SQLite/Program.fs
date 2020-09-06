open System

[<EntryPoint>]
let main argv =
    // DirectSQLite.demo
    // DapperSQLite.demo
    EFCore.demo |> ignore
    0 
