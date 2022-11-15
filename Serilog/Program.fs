//open System
open System.Threading
//open System.Net.Http
//open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
//open Microsoft.Extensions.Logging.ApplicationInsights
//open Microsoft.ApplicationInsights
//open Microsoft.ApplicationInsights.DataContracts
//open Microsoft.ApplicationInsights.WorkerService
//open Serilog
//open Serilog.Sinks.SystemConsole
//open Serilog.Sinks.ApplicationInsights
//open Microsoft.ApplicationInsights.Channel
//open System.Diagnostics
//open Microsoft.Extensions.Logging.Console
open Log

// From https://learn.microsoft.com/en-us/azure/azure-monitor/app/worker-service
// And https://gist.github.com/ninjarobot/eb188314bb667c1ad0a53646be982024

// FIXME: move to secret
//let instrumentationKey = "cc86d35a-e8d5-47e9-ad59-c8b2b4913e1c"

//let appInsightsConnstr =
//    "InstrumentationKey=cc86d35a-e8d5-47e9-ad59-c8b2b4913e1c;IngestionEndpoint=https://francecentral-1.in.applicationinsights.azure.com/;LiveEndpoint=https://francecentral.livediagnostics.monitor.azure.com/"

//let services = ServiceCollection()

//services.AddLogging (fun builder ->
//    builder
//        .SetMinimumLevel(LogLevel.Debug)
//        .AddConsole()
//        .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information)
//        .AddFilter<ApplicationInsightsLoggerProvider>("Category", LogLevel.Information)
//    |> ignore)
//|> ignore

//services.AddApplicationInsightsTelemetryWorkerService(fun options -> options.ConnectionString <- appInsightsConnstr)
//|> ignore

//let serviceProvider = services.BuildServiceProvider()

//let logger =
//    serviceProvider.GetRequiredService<ILogger<Console.ConsoleLoggerProvider>>()

//let telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>()

////let log1 =
////    let logger =
////        LoggerConfiguration()
////            .WriteTo.Console()
////            //.WriteTo.ApplicationInsights()
////            .CreateLogger()

////    logger

let data1 = {| field1 = "ok la"; field2 = 42 |}
let delay = 50000

[<EntryPoint>]
let main _ =
    let (logger, telemetryClient) = createLogger ConsoleAppInsights

    logger.LogInformation(
        "Test info with data: {@data1} and fields {field1} and {field2}",
        data1,
        data1.field1,
        data1.field2
    )

    telemetryClient.Flush()
    Thread.Sleep delay
    0
