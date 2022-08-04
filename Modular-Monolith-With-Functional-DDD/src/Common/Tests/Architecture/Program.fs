open NetArchTest.Rules
open System.Reflection
open FSharp.Reflection

let assemblyPath =
    "../../Domain/bin/Debug/net6.0/MyCompany.Meeting.Common.Domain.dll"

let assembly = Assembly.LoadFrom assemblyPath

let print x = printfn $"{x}"

let printSeparator () =
    printfn $"""{String.replicate 42 "-"}"""

// [<EntryPoint>]
let main1 _ =
    printfn "This program should not be run in standalone mode"

    assembly.GetTypes()
    |> Seq.iter (fun x -> printfn $"{x} {x.Name} {FSharpType.IsModule x} {FSharpType.IsFunction x}")

    printSeparator () |> ignore

    assembly.GetType("MyCompany.Meeting.Common.Domain.ConstrainedType")
    |> (fun x -> printfn $"{x} {x.Name} {FSharpType.IsModule x} {FSharpType.IsFunction x}")

    printSeparator () |> ignore

    assembly
        .GetType("MyCompany.Meeting.Common.Domain.ConstrainedType")
        .GetMethods()
    |> Seq.iter (fun x -> printfn $"{x} {x.Name}")

    printSeparator () |> ignore

    // assembly.GetType("MyCompany.Meeting.Common.Domain.ConstrainedType+dummyCall@55")
    // |> (fun x -> printfn $"{x} {x.Name} {FSharpType.IsModule x} {FSharpType.IsFunction x}")

    // printSeparator () |> ignore

    // Check layers
    assembly.GetReferencedAssemblies()
    |> Seq.iter (fun x -> printfn $"{x}")

    printSeparator () |> ignore

    assembly
        .GetType("MyCompany.Meeting.Common.Domain.ConstrainedType")
        .GetMethod("dummyCall")
    |> (fun x -> printfn $"{x} {x.Name}")

    printSeparator () |> ignore

    assembly.GetTypes()
    |> Seq.filter (FSharpType.IsModule)
    |> Seq.iter (fun x -> printfn $"{x}")

    0

[<EntryPoint>]
let main2 _ =
    printfn "This program should not be run in standalone mode"

    let types = Types.InAssembly(assembly)
    types.GetTypes() |> Seq.iter print
    printSeparator () |> ignore

    types
        .That()
        .ResideInNamespace(("MyCompany"))
        .GetTypes()
    |> Seq.iter print

    printSeparator () |> ignore

    let m = types.That().HaveName("ConstrainedType")

    let f = m.GetTypes() |> Seq.head
    f.GetMethods() |> Seq.iter print

    printSeparator () |> ignore

    m.ShouldNot().HaveDependencyOn(
        "MyCompany.Meeting.Common.Application.Dummy.dummyFunction"
    )
        .GetResult()
        .IsSuccessful
    |> print

    0
