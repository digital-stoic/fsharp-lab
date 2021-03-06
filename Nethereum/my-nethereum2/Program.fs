open System

[<EntryPoint>]
let main argv =
    // TransferEther.printBalance |> Async.RunSynchronously
    // TransferEther.sendTransaction |> Async.RunSynchronously
    // TransferEther.sendWallet |> Async.RunSynchronously
    TransferEther.getBalance |> Async.RunSynchronously
    0
