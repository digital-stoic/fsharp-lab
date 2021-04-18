module Lab.Common

open Farmer
open Farmer.Builders

let resourceGroupName = "Lab"

let locationId = Location.EastUS

let executeDeployment resourceGroupName deployment =
    lazy
        (let deploymentResult =
            deployment
            |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

         match deploymentResult with
         | Ok outputs -> printfn "Success! Output: %A" outputs
         | Error error -> printfn "Failed! %s" error)
