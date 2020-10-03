module Server

open Saturn
open Config
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.Extensions.Logging

let appConfig =
    {| origin = "https://hoppscotch.io"
       loggingLevel = LogLevel.Trace |}

let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let policyConfig (origin: string) (builder: CorsPolicyBuilder) =
    builder.WithOrigins(origin).AllowAnyMethod().AllowAnyHeader()
    |> ignore

let loggingConfig (logLevel: LogLevel) (logging: ILoggingBuilder) =
    logging.SetMinimumLevel(logLevel) |> ignore

let app =
    application {
        pipe_through endpointPipe

        error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_router Router.appRouter
        url "http://0.0.0.0:8085/"
        memory_cache
        use_static "static"
        use_gzip
        use_config (fun _ -> { connectionString = "DataSource=database.sqlite" }) //TODO: Set development time configuration
        use_cors appConfig.origin (policyConfig appConfig.origin)
        logging (loggingConfig appConfig.loggingLevel)
    }

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code
