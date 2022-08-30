module MyCompany.MyApp.Module.Module1.Infrastructure.Azure

open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open MyCompany.MyApp.Common
open MyCompany.MyApp.Common.Application

[<FunctionName("MyCompany-MyApp-Module1-Function1")>]
let run ([<HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)>] req: HttpRequest) (log: ILogger) =
    async {
        log.LogInformation("MyCompany.MyApp.Module1.Function1 triggered...")

        let responseMessage =
            match Domain.Answer.create 42 with
            | Ok a ->
                a
                |> Dto.Answer.fromDomain
                |> Json.Answer.serialize
            | Error e -> failwith e

        return OkObjectResult(responseMessage) :> IActionResult
    }
    |> Async.StartAsTask
