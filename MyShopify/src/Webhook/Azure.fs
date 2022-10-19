module MyShopify.Webhook

open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open System.IO

// TODO: setup AuthorizationLevel.Function
// See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=in-process%2Cfunctionsv2&pivots=programming-language-csharp#http-auth
[<FunctionName("Webhook1")>]
let run ([<HttpTrigger(AuthorizationLevel.Function, "post", Route = null)>] req: HttpRequest) (log: ILogger) =
    async {
        log.LogInformation("MyShopify.Webhook.Webhook1() triggered...")
        let headerKeys = req.Headers.Keys |> Seq.toList
        let headerValues = req.Headers.Values |> Seq.toList
        let headers = List.zip headerKeys headerValues

        headers
        |> List.iter (fun h -> log.LogInformation($"{h}"))

        let body = (new StreamReader(req.Body)).ReadToEnd()
        log.LogInformation($"{body}")
        let responseMessage = {| answer = 42 |}
        return OkObjectResult(responseMessage) :> IActionResult
    }
    |> Async.StartAsTask
