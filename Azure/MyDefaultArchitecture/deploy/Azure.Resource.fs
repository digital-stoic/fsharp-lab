module MyCompany.MyApp.Deploy.Azure.Resource

open System
open Farmer
open Farmer.Builders
open Fake.Core

// TODO: Move to param settings
let appPrefix = "MyCompany-MyApp"
let resourceGroupName = "rg-lab"
let locationId = Location.EastUS
// let locationStr = locationId.ToString() |> String.toLower
let locationStr = "eastus" // FIXME

let servicePlan =
    servicePlan {
        name "myServicePlan1"
        sku WebApp.Sku.Free
        operating_system OS.Linux
    }

let myFunction1 =
    functions {
        name $"{appPrefix}-MyFunction1"
    // link_to_service_plan servicePlan
    }

let deployment = arm { add_resources [ myFunction1 ] }
