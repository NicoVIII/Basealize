namespace Dozenalize

open Basealize

open Dozenalize.Types

[<RequireQualifiedAccess>]
module Display =
    let numBase = 12

    let digit config digit =
        match digit with
        | 0
        | 1
        | 2
        | 3
        | 4
        | 5
        | 6
        | 7
        | 8
        | 9 -> string digit
        | 10 -> config.decChar
        | 11 -> config.elChar
        | _ -> failwithf "%i is not a valid dozenal digit." digit

    let inline number config = Display.number (digit config) numBase
