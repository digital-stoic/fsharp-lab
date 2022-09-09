module MyCompany.MyApp.Warehouse.Infrastructure.Azure

open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open MyCompany.MyApp.Warehouse.Domain

// TODO: setup AuthorizationLevel.Function
// See: https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-http-webhook-trigger?tabs=in-process%2Cfunctionsv2&pivots=programming-language-csharp#http-auth
[<FunctionName("MyCompany-MyApp-Warehouse-Function1")>]
let run ([<HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)>] req: HttpRequest) (log: ILogger) =
    async {
        log.LogInformation("MyCompany.MyApp.Ind triggered...")

        let responseMessage = Cask.create "42" CaskType.Barrel
        //match Domain.Answer.create 42 with
        //| Ok a ->
        //    a
        //    |> Dto.Answer.fromDomain
        //    |> Json.Answer.serialize
        //| Error e -> failwith e

        return OkObjectResult(responseMessage) :> IActionResult
    }
    |> Async.StartAsTask
