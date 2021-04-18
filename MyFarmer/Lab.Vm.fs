module Lab.Vm

open Farmer
open Farmer.Builders

let myVm =
    vm {
        name "vm1"
        username "mat"
        vm_size Vm.Standard_B1s
        operating_systema Vm.WindowsServer_2019Datacenter
        os_disk Vm.StandardSSD_LRS
    }

let deployment = arm { location Lab.Common.locationId }

let execDeployment =
    Lab.Common.executeDeployment Lab.Common.resourceGroupName deployment
