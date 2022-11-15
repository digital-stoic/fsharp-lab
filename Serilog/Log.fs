module Log

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Logging
open Microsoft.Extensions.Logging.ApplicationInsights
open Microsoft.ApplicationInsights

// FIXME: move to secret
let appInsightsConnstr = ""

type LogConfig =
    | Console
    | ConsoleAppInsights

let services = ServiceCollection()

let createLogger logConfig =
    match logConfig with
    | Console ->
        services.AddLogging (fun builder ->
            builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddConsole()
            |> ignore)
        |> ignore
    | ConsoleAppInsights ->
        services.AddLogging (fun builder ->
            builder
                .SetMinimumLevel(LogLevel.Debug)
                .AddConsole()
                .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information)
                .AddFilter<ApplicationInsightsLoggerProvider>("Category", LogLevel.Information)
            |> ignore)
        |> ignore

        services.AddApplicationInsightsTelemetryWorkerService (fun options ->
            options.ConnectionString <- appInsightsConnstr)
        |> ignore

    let serviceProvider = services.BuildServiceProvider()

    let logger =
        serviceProvider.GetRequiredService<ILogger<Console.ConsoleLoggerProvider>>()

    let telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>()
    (logger, telemetryClient)
