module Ch12

open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes

module Dummy =
    let slowFunction () =
        System.Threading.Thread.Sleep 100
        99

    let fastFunction () =
        System.Threading.Thread.Sleep 10
        99

module Harness =
    [<MemoryDiagnoser>]
    type Harness() =
        [<Benchmark>]
        member _.Old() = Dummy.slowFunction ()

        [<Benchmark>]
        member _.New() = Dummy.fastFunction ()

let description = "Performance"

[<EntryPoint>]
let run _ =
    printfn $"{description}..."

    BenchmarkRunner.Run<Harness.Harness>()
    |> printfn "%A"

    0
