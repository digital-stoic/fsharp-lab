#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.IO
open Fake.DotNet
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators

let fakeDirs = [ "./.fake"; "./.ionide" ]

let projectRootDir = "../src"

let appProjectFiles =
    !!(projectRootDir + "/Api/**/*.fsproj")
    ++ (projectRootDir + "/Modules/**/*.fsproj")

let testProjectFiles =
    !!(projectRootDir + "/Common/**/Tests/**/*.fsproj")
    ++ (projectRootDir + "/Modules/**/Tests/**/*.fsproj")
    ++ (projectRootDir + "/Tests/**/*.fsproj")

let dotNetExecProject projectFiles command =
    for projectFile in projectFiles do
        let projectDir = Path.getDirectory projectFile

        Command.RawCommand("dotnet", Arguments.OfArgs command)
        |> CreateProcess.fromCommand
        |> CreateProcess.withWorkingDirectory projectDir
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore

let cleanProject (projectFiles: IGlobbingPattern) logMessage _ =
    Trace.log logMessage
    dotNetExecProject projectFiles [ "clean" ]

let cleanFake fakeDirs _ =
    Trace.log "Cleaning Fake..."
    fakeDirs |> Seq.ofList |> Seq.iter Shell.cleanDir

let cleanApp = cleanProject appProjectFiles "Cleaning App..."

let cleanTest = cleanProject testProjectFiles "Cleaning Tests..."

let buildProject (projectFiles: IGlobbingPattern) logMessage _ =
    Trace.log logMessage
    dotNetExecProject projectFiles [ "build" ]

let buildApp = buildProject appProjectFiles "Building App..."

let buildTest = buildProject testProjectFiles "Building Tests..."

Target.create "CleanFake" <| cleanFake fakeDirs

Target.create "CleanApp" cleanApp

Target.create "CleanTest" cleanTest

Target.create "CleanAll" (fun _ -> Trace.log "Cleaning all...")

Target.create "BuildApp" buildApp

Target.create "BuildTest" buildTest

Target.create "BuildAll" (fun _ -> Trace.log "Building all...")

Target.create "Test" (fun _ ->
    Trace.log "Testing..."
    dotNetExecProject testProjectFiles [ "run"; "--summary" ])

Target.create "Deploy" (fun _ -> Trace.log "Deploying app...")

Target.create "Debug" (fun _ ->
    Trace.log "Debugging..."

    for d in fakeDirs do
        printfn "%s" d
        printfn "%s" (Path.getDirectory d))


"CleanTest"
==> "CleanApp"
// ==> "CleanFake"
==> "CleanAll"

"BuildApp" ==> "BuildTest" ==> "Test"

"BuildApp" ==> "BuildTest" ==> "BuildAll"

"CleanAll" ==> "BuildAll" ==> "Deploy"

Target.runOrDefault "Deploy"
