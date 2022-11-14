//open Microsoft.ApplicationInsights.Channel
//open Microsoft.ApplicationInsights.Extensibility
//open Microsoft.Extensions.DependencyInjection
//open Microsoft.Extensions.Logging
//open Microsoft.Extensions.Logging.ApplicationInsights
open Hopac
//open System
//open System.Diagnostics
//open Microsoft.Extensions.DependencyInjection
//open Microsoft.Extensions.Logging
//open Microsoft.Extensions.Logging.ApplicationInsights
//open Microsoft.ApplicationInsights
//open Microsoft.ApplicationInsights.DataContracts
//open FSharp.Compiler.Service
//open FSharp.Compiler.Interactive
open Logary
open Logary.Configuration
open Logary.Logger
open Logary.Message
open Logary.Targets
//open Microsoft.Extensions.Logging.ApplicationInsights

// FIXME: move to secret
//let appInsightsKey = ""

let appInsightsConnstr = ""


// From https://gist.github.com/ninjarobot/eb188314bb667c1ad0a53646be982024
//let runTelemetry =
//    let services = ServiceCollection()
//    // Lowers the default LogLevel to debug and filters anything below information level from AppInsights.
//    services.AddLogging (fun builder ->
//        builder
//            .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug)
//            .AddConsole()
//            .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information)
//            .AddFilter<ApplicationInsightsLoggerProvider>("Category", LogLevel.Information)
//        |> ignore)
//    |> ignore
//    // Configures the telemetry client as needed when the logger is created.
//    services.AddApplicationInsightsTelemetryWorkerService(fun options -> options.ConnectionString <- appInsightsConnstr)
//    |> ignore

//    let serviceProvider = services.BuildServiceProvider()
//    0

// For the purposes of this script, the logger and telemetry client is resolved directly.
//let logger =
//    serviceProvider.GetRequiredService<ILogger<FSharp.Compiler.Interactive.InteractiveSession>>()
//let logger = serviceProvider.GetRequiredService<Console.ConsoleLoggerProvider("", null)>()

//let telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>()
//logger.LogDebug "Shutting down... (5 second delay for flush)"
//telemetryClient.Flush()
//System.Threading.Thread.Sleep(TimeSpan.FromSeconds 5.)
//logger.LogDebug "Done."

let targetConsole = LiterateConsole.create LiterateConsole.empty "LiterateConsole"

//let logConsole =
//    Config.create "Service1" "Host1"
//    |> Config.target targetConsole
//    |> Config.ilogger (ILogger.Console Debug)
//    |> Config.build
//|> run

let targetAppInsights =
    ApplicationInsights.create
        { InstrumentationKey = appInsightsKey
          DeveloperMode = false
          TrackDependencies = false
          MappingConfiguration = ApplicationInsights.allToTrace }
        "MyApplicationInsights"

let logAppInsights =
    Config.create "Service2" "Host2"
    |> Config.targets [ targetAppInsights
                        targetConsole ]
    |> Config.loggerLevels [ ".*", Verbose ]
    |> Config.ilogger (ILogger.Console Debug)
    |> Config.build
    |> run

type D = { answer: int }

let runLogary =
    printfn "Starting Logary..."
    //let logger = logConsole.getLogger "LogConsole"
    let logger = logAppInsights.getLogger "LogAppInsights"
    logger.debug (eventX "OK la debug 1")
    logger.verbose (eventX "OK la verbose 2")
    logger.info (eventX "OK la info 3")
    logger.warn (eventX "OK la warn 4")
    logger.error (eventX "OK la error 5")
    logger.fatal (eventX "OK la fatal 6")

    logger.info (
        eventX "OK la info gauge 7"
        >> addGauge "Gauge1" (Gauge(Float 0.42, Units.Seconds))
    )

    logger.logWith Info (eventX "OK la info logWith 8")

    let message1 = Message.event Info "OK la info message 9"
    logger.logSimple message1

    logger.info (
        eventX "OK la info setField 10"
        >> setField "Field1" (string "Value1")
        >> setField " Field2" (int 42)
    )

    let message2 =
        event Info "OK la info message format 11"
        |> setField "Fied1" "Value1"
        |> MessageWriter.levelDatetimeMessagePath.format

    logger.logSimple (event Info message2)

    let data = { answer = 42 } :> obj
    let ex = exn "Exception!!!"
    ex.Data.Add("Data", data)

    logger.error (eventX "OK la error exn 12" >> addExn ex)

    event Info "OK la info message format 13"
    |> setField "Fied1" "Value1"
    |> Logger.logSimple logger

    let delay = 50000
    System.Threading.Thread.Sleep delay
    printfn $"Ending Logary after {delay}ms..."

[<EntryPoint>]
let main _ =
    runLogary
    0
