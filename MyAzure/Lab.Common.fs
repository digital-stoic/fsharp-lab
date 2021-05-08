module Lab.Common

open Farmer
open Farmer.Builders

let resourceGroupName = "rg-lab"

let locationId = Location.EastUS

let makeSubnet (subnetName, subnetPrefix) =
    subnet {
        name subnetName
        prefix subnetPrefix
    }

let makeVirtualNetwork vnetName vnetAddresses subnets =
    vnet {
        name vnetName
        add_address_spaces vnetAddresses
        add_subnets subnets

    }

// let makeVmLite vmName userName =
//     vm {
//         name vmName
//         username userName
//         vm_size Basic_A1
//         operating_system CommonImages
//     }

let executeDeployment resourceGroupName deployment =
    lazy
        (let deploymentResult =
            deployment
            |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

         match deploymentResult with
         | Ok outputs -> printfn "Success! Output: %A" outputs
         | Error error -> printfn "Failed! %s" error)
