#r "paket:
nuget Fake.Core.Target
nuget Fake.IO.Filesystem
nuget Farmer"

#load "./Build/Common.fs"
#load "./Build/Build.fs"
#load "./Build/Deploy.fs"

open Fake.Core
open Fake.Core.TargetOperators

Target.create "Build.Clean" Build.clean

Target.create "Deploy.Clean" Deploy.clean

Target.create "Deploy" Deploy.run

Target.create "Full.Clean" <| (fun _ -> Trace.log "Full clean...")

"Build.Clean" ==> "Deploy.Clean" ==> "Full.Clean"

Target.runOrDefault "Deploy"
