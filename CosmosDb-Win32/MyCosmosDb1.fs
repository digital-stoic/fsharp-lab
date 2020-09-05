module MyCosmosDb1

// open Azure.Cosmos
open FSharp.CosmosDb
open FSharp.Control

// let endPointUri = "https://localhost:80801/"

// let primaryKey =
// "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="

// let conn =
// new CosmosClient(endPointUri, primaryKey, {  })

// "AccountEndpoint=https://lab.documents.azure.com:443/;AccountKey=ahEunnlFBzESd8xej6fCL3atNryobvGUuoCkdaIXAAz0oORTOA6oL6RIdUFrJhJSrHH6si1ipdlok8nbFnUfVA==;"
let connStr =
    "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="

type MyData = { Question: string; Answer: int }

let insert<'T> (data: 'T list) =
    connStr
    |> Cosmos.fromConnectionString
    |> Cosmos.database "lab-db"
    |> Cosmos.container "lab-container"
    |> Cosmos.insertMany<'T> data
    |> Cosmos.execAsync<'T>

let find<'T> () =
    connStr
    |> Cosmos.fromConnectionString
    |> Cosmos.database "lab-db"
    |> Cosmos.container "lab-container"
    |> Cosmos.query "SELECT * FROM c WHERE c.Answer = @answer"
    |> Cosmos.parameters [ "@answer", box 42 ]
    |> Cosmos.execAsync<'T>

let data1 =
    [ { Question = "Universe???"
        Answer = 42 } ]

let run (data: MyData list) =
    async {
        let newData = insert<MyData> data
        // let dat = find<MyData> ()
        // do! dat |> AsyncSeq.iter (fun d -> printfn "%A" d)
        return 0
    }
    |> Async.RunSynchronously

let demo =
    run data1 |> ignore
    printfn "F# Lab Comos DB"
    0 |> ignore
