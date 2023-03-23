module MyCompany.MyApp.MyOrder.Application

[<EntryPoint>]
let main _ =
    //printfn "This program should not be run in standalone mode"
    let mutable i = 0

    while true do
        printfn $"{i}..."
        i <- i + 1
        System.Threading.Thread.Sleep(1000)

    0
