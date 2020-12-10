// FIXME: File paths for Windows
#r "paket:
nuget Fake.Core.Target
nuget Fake.IO.FileSystem
"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open System.Text.RegularExpressions

//==============================================================================
// Configuration
//==============================================================================

module getEnv =
    let platform =
        Environment.environVarOrDefault "PLATFORM" "linux-x64"

    let warpFile =
        Environment.environVarOrDefault "WARP_FILE" "warp-packer"

    let warpUrl =
        Environment.environVarOrDefault "WARP_URL" "https://github.com/dgiagio/warp/releases/download/v0.3.0/"
        + platform
        + ".warp-packer"

    let exeFile =
        let projectDir =
            List.last <| String.split '/' __SOURCE_DIRECTORY__

        Environment.environVarOrDefault "EXE_FILE" projectDir

    let publishDir =
        let publishDir =
            __SOURCE_DIRECTORY__
            + "/bin/release/net5.0/"
            + platform
            + "/publish"

        Environment.environVarOrDefault "PUBLISH_DIR" publishDir

    let deployDir =
        Environment.environVarOrDefault "DEPLOY_DIR" "/usr/local/bin"

//==============================================================================
// Helpers
//==============================================================================

let checkCommandExists cmd =
    Command.RawCommand("which", Arguments.OfArgs([ cmd ]))

let runCommand cmd =
    cmd
    |> CreateProcess.fromCommand
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore

//==============================================================================
// Clean
//==============================================================================

let clean _ =
    List.map Fake.IO.Shell.rm_rf [ "bin"; "obj"; ".fake" ]
    |> ignore

//==============================================================================
// Build
//==============================================================================

let dotnetBuild command config =
    Command.RawCommand
        ("dotnet",
         Arguments.OfArgs
             ([ command
                "-r"
                getEnv.platform
                "-c"
                config ]))

let dotnetBuildDebug = dotnetBuild "build" "Debug"

let buildDebug _ = runCommand dotnetBuildDebug

let dotnetBuildOptimized = dotnetBuild "publish" "Release"

let installWarp =
    if Fake.IO.File.exists <| "bin/" + getEnv.warpFile then
        Command.RawCommand("true", Arguments.Empty)
    else
        Command.RawCommand
            ("curl",
             Arguments.OfArgs
                 ([ "-Lo"
                    "bin/" + getEnv.warpFile
                    getEnv.warpUrl ]))

let runWarp =
    Command.RawCommand
        ("bin/" + getEnv.warpFile,
         Arguments.OfArgs
             ([ "--arch"
                getEnv.platform
                "--input_dir"
                getEnv.publishDir
                "--exec"
                getEnv.exeFile
                "--output"
                "bin/" + getEnv.exeFile ]))

let buildOptimized _ =
    runCommand dotnetBuildOptimized
    runCommand (checkCommandExists "curl")
    runCommand installWarp
    runCommand runWarp

//==============================================================================
//Deploy
//==============================================================================

let deploy _ =
    Fake.IO.Shell.copyFile getEnv.deployDir ("bin/" + getEnv.exeFile)

//==============================================================================
// Targets
//==============================================================================

Target.create "Clean" clean

Target.create "BuildDebug" buildDebug

Target.create "BuildOptimized" buildOptimized

Target.create "Deploy" deploy

"BuildOptimized" ==> "Deploy"

Target.runOrDefault "Deploy"
