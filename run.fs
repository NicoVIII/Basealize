open Fake.IO
open RunHelpers
open RunHelpers.Templates
open System.IO

[<RequireQualifiedAccess>]
module Config =
    let srcPath = "./src"
    let testPath = "./tests"
    let examplesPath = "./examples"
    let packPath = "./deploy"

let inline isFsproj (file: string) = file.EndsWith ".fsproj"
let inline isCsproj (file: string) = file.EndsWith ".csproj"
let inline isProj (file: string) = isFsproj file || isCsproj file

let inline getProjectsFromFolder folder =
    folder
    |> Seq.collect (fun folder -> Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories))
    |> Seq.filter isProj

let inline getAllProjects () =
    getProjectsFromFolder [ Config.srcPath; Config.testPath; Config.examplesPath ]

let inline getTestProjects () =
    getProjectsFromFolder [ Config.testPath ]

let inline getSrcProjects () =
    getProjectsFromFolder [ Config.srcPath ]

[<RequireQualifiedAccess>]
module Task =
    let restore () = job {
        DotNet.toolRestore ()

        for project in getAllProjects () do
            DotNet.restore project
    }

    let build config = job {
        for project in getAllProjects () do
            DotNet.build project config
    }

    let test () = parallelJob {
        for project in getTestProjects () do
            DotNet.run project
    }

    let pack version = job {
        Shell.cleanDir Config.packPath

        parallelJob {
            for project in getSrcProjects () do
                DotNet.pack Config.packPath project version
        }
    }

// We determine, what we want to execute
[<EntryPoint>]
let main args : int =
    List.ofArray args
    |> function
        | [ "restore" ] -> Task.restore ()
        | [ "build" ] -> job {
            Task.restore ()
            Task.build Debug
          }
        | [ "test" ] -> job {
            Task.restore ()
            Task.test ()
          }
        | [ "pack"; version ] -> job {
            Task.restore ()
            Task.build Debug
            Task.pack version
          }
        // Catch errors and help
        | [ "pack" ] -> Job.error [ "Usage: dotnet run pack <version>" ]
        | _ -> Job.error [ "Usage: dotnet run <command>"; "Look up available commands in run.fs" ]
    |> Job.execute
