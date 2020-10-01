module MyCosmosDb2

open System
open Microsoft.Azure.Cosmos

type MyType =
    { id: string
      Question: string
      Answer: int
      key: string }

let createDb (client: CosmosClient) dbName =
    client.CreateDatabaseAsync(dbName)
    |> Async.AwaitTask

let deleteDb (db: Database) = db.DeleteAsync() |> Async.AwaitTask

let createContainer (db: Database) (containerName: string) partitionKeyPath =
    db.CreateContainerAsync(containerName, partitionKeyPath)
    |> Async.AwaitTask

let insertItem<'T> (container: Container) (item: 'T) =
    container.CreateItemAsync<'T>(item)
    // let opts = ItemRequestOptions()
    // container.CreateItemAsync<'T>(item, System.Nullable(PartitionKey("1")), opts)
    |> Async.AwaitTask

let sleepDuration = 10000

let connectionString =
    "AccountEndpoint=https://lab.documents.azure.com:443/;AccountKey=P2umBnzCJYUkf7pIXI50lv9DHKBwg5C03h5jna9uCZQToqJ7zj1UwjPFjzi0Yri9MQwevtZOURZ9qJbxez24jA==;"

let options = CosmosClientOptions()

let client =
    new CosmosClient(connectionString, options)

let dbName = "lab-db-test"
// "lab-db-test-" + Guid.NewGuid().ToString()

let containerName = "lab-container"

let partitionKeyPath = "/key"

let myItem1 =
    { id = "1"
      // id = Guid.NewGuid().ToString()
      Question = "Universe?"
      Answer = 42
      key = "1" }

let run =
    async {
        printfn "before"
        let! dbResponse = createDb client dbName
        printfn "Datbase created: %s" dbName
        let db = dbResponse.Database
        let! containerResponse = createContainer db containerName partitionKeyPath
        let container = containerResponse.Container
        printfn "Container created: %A" containerName

        // let! itemResponse = insertItem<MyType> container myItem1

        // printfn "Item inserted %A" itemResponse

        // printfn "%A" container.Id

        // let! result = container.ReadThroughputAsync() |> Async.AwaitTask

        do! Async.Sleep 1000

        let! dbResponse2 = deleteDb db

        printfn "Database deleted: %s" dbName

        printfn "after"

    // return container
    }

let run1 =
    async {
        let! dbResponse = createDb client dbName
        printfn "Datbase created: %s" dbName
        let db = dbResponse.Database
        let! containerResponse = createContainer db containerName partitionKeyPath
        let container = containerResponse.Container
        printfn "Container created: %A" containerName
        return (db, container)
    }

let run3 db =
    async {
        do! Async.Sleep 1000
        let! dbResponse2 = deleteDb db
        printfn "Database deleted: %s" dbName
    }

let run2 container =
    async {
        let! itemResponse = insertItem<MyType> container myItem1
        printfn "Item inserted %A" itemResponse.Resource
    }

let runAll =
    async {
        let! (db, cont) = run1

        let itemResponse =
            insertItem<MyType> cont myItem1
            |> Async.RunSynchronously
        // do run2 cont |> Async.RunSynchronously
        do! run3 db
    }

let demo =
    printfn "Starting..."
    // run |> Async.RunSynchronously
    // let (db, cont) = run1 |> Async.RunSynchronously
    // run2 cont |> Async.RunSynchronously
    // run3 db |> Async.RunSynchronously
    runAll |> Async.RunSynchronously
    printfn "Ending..."
