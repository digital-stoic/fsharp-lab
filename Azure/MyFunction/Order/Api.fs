namespace Api.Order

open System
open System.IO
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs
open Microsoft.Azure.WebJobs.Extensions.Http
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Order.Types
open Order.Place.Application
open Order.Ship.Domain


// TODO: REST API

module Place =
    [<FunctionName("PlaceOrder")>]
    let run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "order/place")>] req: HttpRequest)
        (log: ILogger)
        =
        async {
            log.LogInformation("PlaceOrder...")
            let input = req.Query
            let order = placeOrder input
            let response = order
            return OkObjectResult(response) :> IActionResult
        }
        |> Async.StartAsTask

module Ship =
    [<FunctionName("ShipOrder")>]
    let run
        ([<HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "order/ship")>] req: HttpRequest)
        (log: ILogger)
        =
        async {
            log.LogInformation("ShipOrder...")

            let input: PlacedOrder =
                { OrderId = "1"
                  CustomerInfo = "John Doe"
                  ShippingAddress = "123 Main St"
                  BillingAddress = "123 Main St"
                  Lines = [ "Item 1"; "Item 2" ] }

            let order = shipOrder input
            let response = order
            return OkObjectResult(response) :> IActionResult
        }
        |> Async.StartAsTask
