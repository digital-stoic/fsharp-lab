module MyShopify.Client

open System.IO
open Microsoft.Extensions.Configuration
open Spectre.Console
open FsConfig
open ShopifySharp
open ShopifySharp.Lists

let appSettingsFilename = "appsettings.json"

let appSettingsFilePath =
    $"{Directory.GetCurrentDirectory()}/../../../{appSettingsFilename}"

type Secrets = { shopAccessToken: string }

type Config = { myShopifyUrl: string }

type MyProduct =
    { id: int64 option
      title: string
      bodyHtml: string
      productType: string
      tags: string
      status: string }

let secrets =
    match EnvConfig.Get<Secrets>() with
    | Ok secrets -> secrets
    | Error error ->
        match error with
        | NotFound envVarName -> failwithf "Environment variable %s not found" envVarName
        | BadValue (envVarName, value) -> failwithf "Environment variable %s has invalid value %s" envVarName value
        | NotSupported msg -> failwith msg

let config =
    let configurationRoot =
        ConfigurationBuilder()
            .AddJsonFile(appSettingsFilePath)
            .Build()

    let appConfig =
        AppConfig(configurationRoot)
            .Get<Config>(fun _ x -> x)

    match appConfig with
    | Ok config -> config
    | Error error ->
        match error with
        | NotFound varName -> failwithf "Application configuration variable %s not found" varName
        | BadValue (varName, value) ->
            failwithf "Application configuration variable %s has invalid value %s" varName value
        | NotSupported msg -> failwith msg

let printStatus message =
    async { AnsiConsole.Markup $"[bold aqua]{message}\n[/]" }

let printError message =
    async { AnsiConsole.Markup $"[bold red]Error: {message}\n[/]" }

// FIXME: https://spectreconsole.net/live/status
let printLiveStatus message f =
    AnsiConsole
        .Status()
        .Start(
            message,
            (fun (ctx: StatusContext) ->
                AnsiConsole.MarkupLine("Doing some more work...")
                // ctx.Status("ok")
                f)
        )

let toMyProduct (p: Product) =
    let id =
        if p.Id.HasValue then
            Some p.Id.Value
        else
            None

    { id = id
      title = p.Title
      bodyHtml = p.BodyHtml
      productType = p.ProductType
      tags = p.Tags
      status = p.Status }

let run =
    async {
        let productService = ProductService(config.myShopifyUrl, secrets.shopAccessToken)
        let! productCount = productService.CountAsync() |> Async.AwaitTask
        printfn $"Product count: {productCount}"
        let! productList = productService.ListAsync() |> Async.AwaitTask
        printfn $"Has next page: {productList.HasNextPage}"
        let products = productList.Items |> Seq.toList
        let myProducts = products |> List.map toMyProduct
        do! (printStatus "Products before:")
        printfn $"{myProducts}"
        let product1 = products |> List.head
        let newProduct1 = product1
        //newProduct1.Tags <- "tag3"

        //let! updatedProduct1 =
        //    productService.UpdateAsync(product1.Id.Value, newProduct1)
        //    |> Async.AwaitTask

        //do! (printStatus "Product #1 after:")
        //printfn $"{product1}"

        let metaFieldService =
            MetaFieldService(config.myShopifyUrl, secrets.shopAccessToken)

        let! metaFieldList =
            metaFieldService.ListAsync(product1.Id.Value, "products")
            |> Async.AwaitTask

        let metaFields = metaFieldList.Items |> Seq.toList

        let metaField1 =
            metaFields
            |> List.find (fun m -> m.Key = "metafield")

        let newMetaField1 = metaField1
        newMetaField1.Value <- $"{newMetaField1.Value}X"

        let! updatedMetaField1 =
            metaFieldService.UpdateAsync(newMetaField1.Id.Value, newMetaField1)
            |> Async.AwaitTask

        //printfn $"{m1.Id}, {m1.Key}, {m1.Value}"



        return 0
    }
