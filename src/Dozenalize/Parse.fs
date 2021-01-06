namespace Dozenalize

open System.Text.RegularExpressions

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
            let (negative, number) =
                if number.StartsWith "-" then
                    true, number.Split [| char "-" |] |> String.concat ""
                else
                    false, number

            let separator = [| char ","; char "." |]

            let startExponent =
                match number.IndexOfAny separator with
                // Haben wir keinen Separator, nehmen wir die Länge her
                | -1 -> String.length number - 1
                // Ansonsten nehmen wir die Länge bis zum Separator
                | x -> x - 1

            // String without separators and sign
            let number' =
                number.Split separator |> String.concat ""

            number'.ToCharArray()
            |> Array.map string
            |> Array.fold
                (fun (sum, exponent) char ->
                    let sum =
                        digit config char
                        |> (*) (pown 12. exponent)
                        |> (+) sum

                    (sum, exponent - 1))
                (0., startExponent)
            |> fst
            // If we have a negative number, we have to make it negative
            |> fun sum -> if negative then sum * -1. else sum
            |> Some
        | _ -> None

    let number config (number: string) =
        match tryNumber config number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
