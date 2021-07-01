namespace Niftimalize

open Basealize

[<RequireQualifiedAccess>]
module Display =
    let numBase = 36

    let letters = [ 'A' .. 'Z' ] |> List.map string

    let digit digit =
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
        | i when i >= 10 && i <= 35 -> letters.[i - 10]
        | _ -> failwithf "%i is not a valid niftimal digit." digit

    let inline number precision = Display.number digit numBase precision
