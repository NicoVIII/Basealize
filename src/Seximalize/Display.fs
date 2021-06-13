namespace Seximalize

open Basealize

[<RequireQualifiedAccess>]
module Display =
    let numBase = 6

    let digit digit =
        match digit with
        | 0
        | 1
        | 2
        | 3
        | 4
        | 5 -> string digit
        | _ -> failwithf "%i is not a valid seximal digit." digit

    let inline number precision = Display.number digit numBase precision
