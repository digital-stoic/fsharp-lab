[<EntryPoint>]
let main argv =
    // Template1.executeDeployment.Force()
    // Template1.debug.Force()
    // Lab.Vnet.executeDeployment.Force()
    // Lab.Vm.executeDeployment.Force()
    Lab.WebApp.executeDeployment.Force()
    0
