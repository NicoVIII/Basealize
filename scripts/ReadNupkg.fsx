/// Helper skript to get Metadata of generated nupkg files (run pack script first)
/// Call with dotnet fsi <script_path>

#r "nuget: NuGet.Packaging"

open NuGet.Packaging
open System.IO

// Config
let nupkgDir = "../deploy"

// Helper functions
let getFileFromDir dir = (Directory.GetFiles dir).[0]

let toAbsPath relPath =
  Path.Combine(__SOURCE_DIRECTORY__, relPath)

let streamFromFile path = new FileStream(path, FileMode.Open)

let readStream (stream: FileStream) = new PackageArchiveReader(stream)

// Create reader
let reader =
  toAbsPath nupkgDir
  |> getFileFromDir
  |> streamFromFile
  |> readStream

let nuspec = reader.NuspecReader

// Print Info
printfn $"ID: {nuspec.GetId()}"
printfn $"Version: {nuspec.GetVersion()}"
printfn $"Description: {nuspec.GetDescription()}"
printfn $"Authors: {nuspec.GetAuthors()}"

printfn "Dependencies:"

for dependencyGroup in nuspec.GetDependencyGroups() do
  printfn $" - {dependencyGroup.TargetFramework.GetShortFolderName()}"

  for dependency in dependencyGroup.Packages do
    printfn $"   > {dependency.Id} {dependency.VersionRange}"

printfn "Files:"

for file in reader.GetFiles() do
  printfn $" - {file}"
