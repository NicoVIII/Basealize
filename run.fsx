#!/bin/dotnet fsi

#r "nuget: Fake.DotNet.Cli"
#r "nuget: FsToolkit.ErrorHandling"

open System.IO

open Fake.DotNet

open FsToolkit.ErrorHandling

module Config =
    let testFolder = "tests"

// FsToolkit does not provide the kleisli composition
let (>=>) fun1 fun2 = fun1 >> (Result.bind fun2)

module Result =
    /// Collapses two of this simple results into one
    let collapse res1 res2 =
        match res1, res2 with
        | Ok (), Ok () -> Ok()
        | Error (), _
        | _, Error () -> Error()

// I want to use the Result type for results of exec
let dotnet command args () =
    DotNet.exec id command args
    |> (fun result -> result.OK)
    |> function
    | true -> Ok()
    | false -> Error()

module Commands =
    let restore =
        dotnet "tool" "restore" >=> dotnet "restore" ""

    let build = dotnet "build" ""

    let test () =
        Directory.EnumerateDirectories Config.testFolder
        |> Seq.map Directory.EnumerateFiles
        |> Seq.concat
        |> Seq.filter (fun file -> file.EndsWith ".fsproj")
        |> Seq.map
            (fun project ->
                printfn "\nRun tests in %s:" project

                dotnet "run" $"--project {project}" ())
        |> Seq.reduce Result.collapse

// We determine, what we want to execute
let execute =
    // We read the command from arguments
    Array.tryItem 1 fsi.CommandLineArgs
    |> Option.defaultValue ""
    // We determine which commands to run
    |> function
    | "restore" -> Commands.restore
    | "build" -> Commands.build
    | "test" -> Commands.build >=> Commands.test
    | _ ->
        (fun _ ->
            printfn "Usage: (./run.fsx | dotnet fsi run.fsx) <command>"
            printfn "Look up available commands in run.fsx"
            Error())

// We execute it and map the result to an exit code
execute ()
|> function
| Ok () -> 0
| Error () -> 1
|> exit
