namespace Seximalize

open System.Text.RegularExpressions

open Basealize

[<RequireQualifiedAccess>]
module Parse =
    let (|ValidNumber|_|) numberString =
        let digit = @"[0-5]"

        let m =
            Regex.Match(numberString, @"^-?" + digit + @"+([.,]" + digit + @"*)?$")

        if m.Success then
            Some numberString
        else
            None

    let inline digit digit =
        match digit with
        | "0" -> 0.
        | "1" -> 1.
        | "2" -> 2.
        | "3" -> 3.
        | "4" -> 4.
        | "5" -> 5.
        | _ -> failwithf "%s is not a valid seximal digit." digit

    let tryNumber (number: string) =
        match number with
        | ValidNumber _ -> Parse.parseNumber digit 6. number |> Some
        | _ -> None

    let number (number: string) =
        match tryNumber number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
