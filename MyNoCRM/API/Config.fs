module Config

open System.IO
open Microsoft.Extensions.Configuration
open FsConfig

let appSettingsFilename = "appsettings.json"

let appSettingsFilePath =
    $"{Directory.GetCurrentDirectory()}/../../../{appSettingsFilename}"

type Secrets =
    { cosmosKey: string
      nocrmApiKey: string }

type Settings =
    { accountEndPoint: string
      databaseName: string
      containerName: string
      partitionKey: string }

let secrets =
    match EnvConfig.Get<Secrets>() with
    | Ok secrets -> secrets
    | Error error ->
        match error with
        | NotFound envVarName -> failwithf "Environment variable %s not found" envVarName
        | BadValue (envVarName, value) -> failwithf "Environment variable %s has invalid value %s" envVarName value
        | NotSupported msg -> failwith msg

let settings =
    let configurationRoot =
        ConfigurationBuilder()
            .AddJsonFile(appSettingsFilePath)
            .Build()

    let appConfig =
        AppConfig(configurationRoot)
            .Get<Settings>(fun _ x -> x)

    match appConfig with
    | Ok config -> config
    | Error error ->
        match error with
        | NotFound varName -> failwithf "Application configuration variable %s not found" varName
        | BadValue (varName, value) ->
            failwithf "Application configuration variable %s has invalid value %s" varName value
        | NotSupported msg -> failwith msg
