module CosmosDB

open Microsoft.Azure.Cosmos
open Hopac
open HttpFs.Client
open Json
open Data
open Config

let asyncRequest apiKey =
    let baseUrl = "https://fcask.nocrm.io/api/v2"
    let method = Get
    let url = $"{baseUrl}/leads"

    async {
        let request =
            Request.createUrl method url
            |> Request.setHeader (Custom("X-API-KEY", apiKey))
            |> Request.responseAsString

        let! response = request |> Hopac.startAsTask
        return response
    }

let createDatabase (client: CosmosClient) databaseName =
    client
        .CreateDatabaseIfNotExistsAsync(
            databaseName
        )
        .Result
        .Database

let createContainer (database: Database) containerName partitionKey =
    database
        .CreateContainerIfNotExistsAsync(
            // TODO: check if serverless thtoughput
            ContainerProperties(containerName, partitionKey)
        )
        .Result
        .Container

let upsertItem (container: Container) item (key: string) =
    container
        .UpsertItemAsync(
            item,
            new PartitionKey(key)
        )
        .Result

let asyncRun =
    async {
        printfn "Starting CosmosDB..."

        let jsonLeads =
            asyncRequest secrets.nocrmApiKey
            |> Async.RunSynchronously

        let leads = tryDecodeLeads jsonLeads
        // TODO: use 'use'?
        let client = new CosmosClient(settings.accountEndPoint, secrets.cosmosKey)
        let database = createDatabase client settings.databaseName

        let container =
            createContainer database settings.containerName settings.partitionKey
        // TODO: bulk insert?
        // See https://learn.microsoft.com/en-us/azure/cosmos-db/nosql/tutorial-dotnet-bulk-import
        //let lead = upsertItem container leads[0] leads[0].id
        let results =
            leads
            |> List.map (fun l -> upsertItem container l l.id)

        printfn $"{results}"


        printfn "Ending CosmosDB..."

    }
