// From https://github1s.com/Azure-Samples/cosmos-db-sql-api-dotnet-samples

// For more information see https://aka.ms/fsharp-console-apps
[<EntryPoint>]
let main _ =
    printfn "Starting MyNoCRM..."
    CosmosDB.asyncRun |> Async.RunSynchronously
    0
