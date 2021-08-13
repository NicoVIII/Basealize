open System.IO

open Fake.DotNet
open Fake.IO

module Config =
    let srcFolder = "src"
    let testFolder = "tests"
    let examplesFolder = "examples"
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
let isCsproj (file: string) = file.EndsWith ".csproj"
let isProj (file: string) = isFsproj file || isCsproj file

let handleProjects folder handler () =
    folder
    |> Seq.map Directory.EnumerateDirectories
    |> Seq.concat
    |> Seq.map Directory.EnumerateFiles
    |> Seq.concat
    |> Seq.filter isProj
    |> Seq.map (fun proj -> handler proj ())
    |> Seq.reduce Result.collapse

let handleAllProjects = handleProjects [ Config.srcFolder; Config.testFolder; Config.examplesFolder ]
let handleTestProjects = handleProjects [ Config.testFolder ]

module Commands =
    let restore =
        handleAllProjects (dotnet "restore")

    let build = handleAllProjects (dotnet "build")

    let test =
        fun proj _ ->
            printfn "\nRun tests in %s:" proj

            dotnet "run" $"--project \"{proj}\"" ()
        |> handleTestProjects

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
[<EntryPoint>]
let execute args: int =
    // We read the command from arguments
    let command =
        Array.tryItem 0 args |> Option.defaultValue ""

    // Read arguments for commands
    let commandArgs =
        if Array.length args > 1 then
            Array.skip 1 args |> Array.toList
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
                printfn "Usage: dotnet run <command>"
                printfn "Look up available commands in run.fs"
                Error())
    |> (fun fnc -> fnc ())
    |> function
        | Ok () -> 0
        | Error () -> 1
