namespace Basealize

open System.Text.RegularExpressions

[<RequireQualifiedAccess>]
module Parse =
    let validNumber digitRegex numberString =
        Regex.Match(
            numberString,
            @"^-?"
            + digitRegex
            + @"+([.,]"
            + digitRegex
            + @"*)?$"
        )
        |> (fun m -> m.Success)
        |> function
            | true -> Some numberString
            | false -> None

    let parseNumber digit numBase (number: string) =
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
        let number' = number.Split separator |> String.concat ""

        number'.ToCharArray()
        |> Array.map string
        |> Array.fold
            (fun (sum, exponent) char ->
                let sum =
                    digit char
                    |> (*) (pown numBase exponent)
                    |> (+) sum

                (sum, exponent - 1))
            (0., startExponent)
        |> fst
        // If we have a negative number, we have to make it negative
        |> fun sum -> if negative then sum * -1. else sum
