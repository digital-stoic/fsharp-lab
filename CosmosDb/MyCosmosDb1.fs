module MyCosmosDb1

// FIXME: Fsharp.Configuration not compatible with .Net Core
// FIXME FsConfig requires manual installation of TypeShape
open System.IO
open Microsoft.Extensions.Configuration
open FsConfig
open FSharp.CosmosDb
open FSharp.Control

type Config =
    { [<DefaultValue("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")>]
      DbConnectionString: string }

let appSettingsFilename = "appsettings.json"

let Config =
    let configurationRoot =
        ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(appSettingsFilename).Build()

    let appConfig =
        AppConfig(configurationRoot).Get<Config>(fun _ s -> s)

    match appConfig with
    | Ok config -> config
    | Error error ->
        match error with
        | NotFound envVarName -> failwithf "Environment variable %s not found" envVarName
        | BadValue (envVarName, value) -> failwithf "Environment variable %s has invalid value: %s" envVarName value
        | NotSupported msg -> failwith msg

// TODO: UUID for Id
// TODO: Check if field names are case sensitive
[<CLIMutable>]
type MyData =
    { id: string
      Question: string
      Answer: int
      key: int }

let insert<'T> data =
    Config.DbConnectionString
    |> Cosmos.fromConnectionString
    |> Cosmos.database "lab-db"
    |> Cosmos.container "lab-container"
    |> Cosmos.insertMany<'T> data
    |> Cosmos.execAsync

let find<'T> () =
    Config.DbConnectionString
    |> Cosmos.fromConnectionString
    |> Cosmos.database "lab-db"
    |> Cosmos.container "lab-container"
    |> Cosmos.query "SELECT *"
    |> Cosmos.execAsync<'T>

let data1 =
    [ { id = "5"
        Question = "Universe?"
        Answer = 42
        key = 0 } ]

let run (data: MyData list) =
    async {
        let newData = insert<MyData> data
        // let dat = find<MyData> ()
        // do! dat |> AsyncSeq.iter (fun d -> printfn "%A" d)
        return 0
    }
    |> Async.RunSynchronously

let demo =
    // run data1 |> ignore
    insert<MyData> data1
    |> AsyncSeq.toListAsync
    |> Async.RunSynchronously
    |> ignore
    printfn "F# Lab Comos DB"
    0 |> ignore
