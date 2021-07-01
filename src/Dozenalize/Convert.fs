namespace Dozenalize

open Basealize

module Constants =
    let numBase = 12
    let numBaseFloat = numBase |> float

[<RequireQualifiedAccess>]
module Display =
    let digit config digit =
        match digit with
        | i when i >= 0 && i <= 9 -> string digit
        | 10 -> config.decChar
        | 11 -> config.elChar
        | _ -> failwithf "%i is not a valid dozenal digit." digit

    let inline number config =
        Display.number (digit config) Constants.numBase

[<RequireQualifiedAccess>]
module Parse =
    let validNumber config =
        Parse.validNumber (@"[0-9" + config.decChar + config.elChar + @"]")

    let (|ValidNumber|_|) = validNumber

    let tryDigit config digit =
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
        | char when char = config.decChar -> Some 10.
        | char when char = config.elChar -> Some 11.
        | _ -> None

    let digit config digit =
        tryDigit config digit
        |> Option.defaultWith
            (fun () ->
                failwithf "%s is not a valid dozenal digit for given config." digit)

    let tryNumber config (number: string) =
        match number with
        | ValidNumber config _ ->
            Parse.parseNumber (digit config) Constants.numBaseFloat number
            |> Some
        | _ -> None

    let number config (number: string) =
        match tryNumber config number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
