#r "paket:
nuget Fake.Core.Target
nuget Fake.IO.System"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
// open Fake.IO.File

let warpFile = "warp-packer"

// TODO: Portable platform for Warp
let warpUrl =
    "https://github.com/dgiagio/warp/releases/download/v0.3.0/linux-x64.warp-packer"

let checkCommandExists cmd =
    Command.RawCommand("which", Arguments.OfArgs([ cmd ]))

let runCommand cmd =
    cmd
    |> CreateProcess.fromCommand
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

let dotnetBuild =
    Command.RawCommand("dotnet", Arguments.OfArgs([ "build"; "-c"; "release" ]))

let installWarp =
    if Fake.IO.File.exists <| "bin/" + warpFile
    then Command.RawCommand("true", Arguments.Empty)
    else Command.RawCommand("curl", Arguments.OfArgs([ "-Lo"; "bin/" + warpFile; warpUrl ]))

let runWarp =
    let platform = "linux-x64"

    let publishPath =
        "/mnt/c/data/dev/fsharp-lab/Scripting/single-executable/bin/release/net5.0/linux-x64/publish"

    let exeFile = "single-executable"
    let outputFile = "bin/" + exeFile
    Command.RawCommand
        ("bin/" + warpFile,
         Arguments.OfArgs
             ([ "--arch"
                platform
                "--input_dir"
                publishPath
                "--exec"
                exeFile
                "--output"
                outputFile ]))

let build _ =
    runCommand dotnetBuild
    runCommand (checkCommandExists "curl")
    runCommand installWarp
    runCommand runWarp

Target.create "Clean" (fun _ -> Trace.log "Cleaning...")

Target.create "BuildDebug" build

Target.create "BuildOptimized" build

Target.create "Deploy" (fun _ -> Trace.log "Deploying...")

"Clean" ==> "BuildOptimized" ==> "Deploy"

Target.runOrDefault "Deploy"
