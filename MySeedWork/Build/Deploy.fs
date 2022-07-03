module Deploy

open Farmer
open Farmer.Builders
open Common

//==============================================================================
// Parameters
//==============================================================================

let locationId = Location.EastUS
let resourceGroupName = "rg-lab"
let storageAccountName = "stdigitalstoiclab2"

//==============================================================================
// Resources
//==============================================================================

let storageAccount = storageAccount { name storageAccountName }

let deployment =
    arm {
        location locationId
        add_resource storageAccount
    }

//==============================================================================
// Helpers
//==============================================================================

let runDeployment resourceGroupName deployment =
    let deploymentResult =
        deployment
        |> Deploy.tryExecute resourceGroupName Deploy.NoParameters

    match deploymentResult with
    | Ok outputs -> printfn "Success! Output: %A" outputs
    | Error error -> printfn "Failed! %s" error

let az args = shellCommand "az" args "."

//==============================================================================
// Targets
//==============================================================================

let clean _ =
    az [ "group"
         "delete"
         "--name"
         resourceGroupName
         "--yes" ]

let run _ =
    runDeployment resourceGroupName deployment
