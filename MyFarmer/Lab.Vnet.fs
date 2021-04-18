module Lab.Vnet

open Farmer
open Farmer.Builders

let resourceGroupName = "Lab"

let locationId = Location.EastUS

let myVnet =
    vnet {
        name "vnet1"
        add_address_spaces [ "10.1.0.0/16" ]

        add_subnets [ subnet {
                          name "subnet1"
                          prefix "10.1.1.0/24"
                      }
                      subnet {
                          name "subnet2"
                          prefix "10.1.2.0/24"
                      } ]
    }

let deployment =
    arm {
        location locationId
        add_resource myVnet
    }

let executeDeployment =
    lazy
        (let deploymentResult =
            deployment
            |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

         match deploymentResult with
         | Ok outputs -> printfn "Success! Output: %A" outputs
         | Error error -> printfn "Failed! %s" error)
