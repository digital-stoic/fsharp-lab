namespace UnitConverter

open System
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open UnitConverter.Domain


module Api =
    [<FunctionName("UnitConverter")>]
    let run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)>] req: HttpRequest)
        (log: ILogger)
        =
        async {
            log.LogInformation("F# HTTP trigger function processed a request.")

            let celsiusOpt =
                if req.Query.ContainsKey("celsius") then
                    Some(int req.Query["celsius"].[0])
                else
                    None

            let response =
                match celsiusOpt with
                | Some c -> celsiusToFahrenheit c |> string
                | None -> ""

            return OkObjectResult(response) :> IActionResult
        }
        |> Async.StartAsTask
