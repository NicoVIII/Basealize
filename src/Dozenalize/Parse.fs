namespace Dozenalize

open System.Text.RegularExpressions

open Basealize

open Dozenalize.Types

[<RequireQualifiedAccess>]
module Parse =
    let (|ValidNumber|_|) config numberString =
        let digit =
            @"[0-9" + config.decChar + config.elChar + @"]"

        let m =
            Regex.Match(numberString, @"^-?" + digit + @"+([.,]" + digit + @"*)?$")

        if m.Success then
            Some numberString
        else
            None

    let inline digit config digit =
        match digit with
        | "0" -> 0.
        | "1" -> 1.
        | "2" -> 2.
        | "3" -> 3.
        | "4" -> 4.
        | "5" -> 5.
        | "6" -> 6.
        | "7" -> 7.
        | "8" -> 8.
        | "9" -> 9.
        | char when char = config.decChar -> 10.
        | char when char = config.elChar -> 11.
        | _ -> failwithf "%s is not a valid dozenal digit for given config." digit

    let tryNumber config (number: string) =
        match number with
        | ValidNumber config _ ->
            Parse.parseNumber (digit config) 12. number
            |> Some
        | _ -> None

    let number config (number: string) =
        match tryNumber config number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
