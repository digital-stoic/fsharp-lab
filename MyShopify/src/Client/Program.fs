open MyShopify.Client

[<EntryPoint>]
let main _ =
    printfn "Starting MyShopify.Client..."

    MyShopify.Client.run
    |> Async.RunSynchronously
    |> ignore

    0
