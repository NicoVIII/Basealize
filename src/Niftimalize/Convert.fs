namespace Niftimalize

open Basealize

module Constants =
    let numBase = 36
    let numBaseFloat = numBase |> float

    let letters = [ 'A' .. 'Z' ] |> List.map string

[<RequireQualifiedAccess>]
module Display =
    let digit digit =
        match digit with
        | i when i >= 0 && i <= 9 -> string digit
        | i when i >= 10 && i <= 35 -> Constants.letters.[i - 10]
        | _ -> failwithf "%i is not a valid niftimal digit." digit

    let inline number precision =
        Display.number digit Constants.numBase precision

[<RequireQualifiedAccess>]
module Parse =
    let validNumber = Parse.validNumber @"[0-9A-Z]"
    let (|ValidNumber|_|) = validNumber

    let tryDigit digit =
        match digit with
        | "0" -> Some 0.
        | "1" -> Some 1.
        | "2" -> Some 2.
        | "3" -> Some 3.
        | "4" -> Some 4.
        | "5" -> Some 5.
        | "6" -> Some 6.
        | "7" -> Some 7.
        | "8" -> Some 8.
        | "9" -> Some 9.
        | c when List.contains c Constants.letters ->
            List.findIndex ((=) c) Constants.letters |> (+) 10 |> (float >> Some)
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
