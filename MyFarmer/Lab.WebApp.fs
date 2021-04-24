module Lab.WebApp

open Farmer
open Farmer.Builders

let myWebApp =
    webApp {
        name "myWebApp-lab-matieux"
        zip_deploy @"tmp/deploy"
    }

let deployment =
    arm {
        location Lab.Common.locationId
        add_resource myWebApp
    }

let executeDeployment =
    Lab.Common.executeDeployment Lab.Common.resourceGroupName deployment
