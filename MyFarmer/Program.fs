[<EntryPoint>]
let main argv =
    Template1.executeDeployment.Force()
    // Template1.debug.Force()
    0
