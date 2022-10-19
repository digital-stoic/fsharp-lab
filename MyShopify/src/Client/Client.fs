module MyShopify.Client

open System.IO
open Microsoft.Extensions.Configuration
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

        let myProducts =
            productList.Items
            |> Seq.map toMyProduct
            |> Seq.toList

        printfn $"{myProducts}"

        return myProducts
    }
