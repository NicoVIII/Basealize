#!/bin/dotnet fsi

#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.IO.FileSystem"

open System.IO

open Fake.DotNet
open Fake.IO

module Config =
    let srcFolder = "src"
    let testFolder = "tests"
    let packFolder = "deploy"

// kleisli composition operator for chaining
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
    printfn $"> dotnet {command} {args}"

    DotNet.exec id command args
    |> (fun result -> result.OK)
    |> function
        | true -> Ok()
        | false -> Error()

let isFsproj (file: string) = file.EndsWith ".fsproj"

module Commands =
    let restore =
        dotnet "tool" "restore" >=> dotnet "restore" ""

    let build = dotnet "build" ""

    let test () =
        Directory.EnumerateDirectories Config.testFolder
        |> Seq.map Directory.EnumerateFiles
        |> Seq.concat
        |> Seq.filter isFsproj
        |> Seq.map
            (fun project ->
                printfn "\nRun tests in %s:" project

                dotnet "run" $"--project \"{project}\"" ())
        |> Seq.reduce Result.collapse

    /// Packs everything for nuget with given version
    let pack version () =
        let outDir = Config.packFolder

        // Clean up output folder
        Directory.delete outDir

        // We pack every project in the src folder
        Directory.EnumerateDirectories Config.srcFolder
        |> Seq.map Directory.EnumerateFiles
        |> Seq.concat
        |> Seq.filter isFsproj
        |> Seq.map (fun project -> dotnet "pack" $"-c Release -o \"{outDir}\" /p:Version=%s{version} \"{project}\"" ())
        |> Seq.reduce Result.collapse

// We determine, what we want to execute
let execute args =
    // We read the command from arguments
    let command =
        Array.tryItem 1 args |> Option.defaultValue ""

    // Read arguments for commands
    let commandArgs =
        if Array.length args > 2 then
            Array.skip 2 args |> Array.toList
        else
            []

    (command, commandArgs)
    // We determine which commands to run
    |> function
        | "restore", [] -> Commands.restore
        | "build", [] -> Commands.build
        | "test", [] -> Commands.build >=> Commands.test
        | "pack", [ version ] -> Commands.build >=> Commands.pack version
        // Catch errors and help
        | "pack", [] ->
            (fun _ ->
                printfn "pack command needs one argument, which is the version to pack for."
                Error())
        | _ ->
            (fun _ ->
                printfn "Usage: (./run.fsx | dotnet fsi run.fsx) <command>"
                printfn "Look up available commands in run.fsx"
                Error())

// We execute it and map the result to an exit code
execute fsi.CommandLineArgs ()
|> function
    | Ok () -> 0
    | Error () -> 1
|> exit
