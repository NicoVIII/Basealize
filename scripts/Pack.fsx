#r "nuget: Fake.DotNet.Cli"
#r "nuget: Fake.IO.FileSystem"

open Fake.DotNet
open Fake.IO
open System.IO

// Arguments
if fsi.CommandLineArgs.Length <> (1 + 1) then
  failwith "Wrong number of arguments given. Needs exactly one: version"

let version = fsi.CommandLineArgs.[1]

// Config
let project = "src/Seximalize/Seximalize.fsproj"

let outDir =
  Path.Combine(__SOURCE_DIRECTORY__, "../deploy")

let customParams = $"/p:Version=%s{version}"

// Helper functions
let packOptions options: DotNet.PackOptions =
  { options with
      Configuration = DotNet.BuildConfiguration.Release
      OutputPath = Some outDir
      Common = DotNet.Options.withCustomParams (Some customParams) options.Common }

// Clean & Pack
Directory.delete outDir
DotNet.pack packOptions project
