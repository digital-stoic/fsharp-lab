module Template1

open Farmer
open Farmer.Builders

let debug _lazy = printfn "Template1..."

let resourceGroupName = "Lab"

let myWebApp =
    webApp { name "MyFarmerTemplate1WebApp" }

let deployment =
    arm {
        location Location.EastUS
        add_resource myWebApp
    }

let writeDeployment x =
    deployment |> Writer.quickWrite "Template1WebApp"

let executeDeployment _lazy =
    let deploymentResult =
        deployment
        |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

    match deploymentResult with
    | Ok outputs -> printfn "Success! Output: %A" outputs
    | Error error -> printfn "Failed! %s" error
