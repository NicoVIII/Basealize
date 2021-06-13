open System
open Expecto
open Tests

[<EntryPoint>]
let main args =
    let config = defaultConfig
    runTestsWithArgs config args tests
