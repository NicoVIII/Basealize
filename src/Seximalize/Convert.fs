namespace Seximalize

open Basealize

module Constants =
    let numBase = 6
    let numBaseFloat = numBase |> float

[<RequireQualifiedAccess>]
module Display =
    let digit digit =
        match digit with
        | i when i >= 0 && i <= 5 -> string digit
        | _ -> failwithf "%i is not a valid seximal digit." digit

    let inline number precision =
        Display.number digit Constants.numBase precision

[<RequireQualifiedAccess>]
module Parse =
    let validNumber = Parse.validNumber @"[0-5]"
    let (|ValidNumber|_|) = validNumber

    let tryDigit digit =
        match digit with
        | "0" -> Some 0.
        | "1" -> Some 1.
        | "2" -> Some 2.
        | "3" -> Some 3.
        | "4" -> Some 4.
        | "5" -> Some 5.
        | _ -> None

    let digit digit =
        tryDigit digit
        |> Option.defaultWith (fun () -> failwithf "%s is not a valid seximal digit." digit)

    let tryNumber (number: string) =
        match number with
        | ValidNumber _ -> Parse.parseNumber digit Constants.numBaseFloat number |> Some
        | _ -> None

    let number (number: string) =
        match tryNumber number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
