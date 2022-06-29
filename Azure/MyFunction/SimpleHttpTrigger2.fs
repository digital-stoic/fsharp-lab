namespace AzureFunctions.Function

open System
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

module SimpleHttpTrigger2 =
    [<FunctionName("SimpleHttpTrigger2")>]
    let run ([<HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)>]req: HttpRequest) (log: ILogger) =
        async {
            log.LogInformation("F# HTTP trigger 2 function processed a request.")
            let responseMessage = "Hello world 2!\n"
            return OkObjectResult(responseMessage) :> IActionResult
        } |> Async.StartAsTask