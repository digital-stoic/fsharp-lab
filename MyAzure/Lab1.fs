module Lab1

open Farmer
open Farmer.Builders
open Lab.Common

let vnet1Name = "vnet-1"
let vnet1Addresses = [ "10.1.0.0/16" ]
let subnet1s = [ ("snet-1", "10.1.1.0/24") ]
let vnet2Name = "vnet-2"
let vnet2Addresses = [ "10.2.0.0/16" ]
let subnet2s = [ ("snet-2", "10.2.1.0/24") ]

let vnet1 =
    makeVirtualNetwork vnet1Name vnet1Addresses (List.map makeSubnet subnet1s)

let vnet2 =
    makeVirtualNetwork vnet2Name vnet2Addresses (List.map makeSubnet subnet2s)

let deployment =
    arm {
        location locationId
        add_resource vnet1
        add_resource vnet2
    }

let executeDeployment =
    lazy
        (let deploymentResult =
            deployment
            |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

         match deploymentResult with
         | Ok outputs -> printfn "Success! Output: %A" outputs
         | Error error -> printfn "Failed! %s" error)
