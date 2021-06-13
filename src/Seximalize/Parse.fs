namespace Seximalize

open System.Text.RegularExpressions

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
        | ValidNumber _ ->
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
                        digit char |> (*) (pown 6. exponent) |> (+) sum

                    (sum, exponent - 1))
                (0., startExponent)
            |> fst
            // If we have a negative number, we have to make it negative
            |> fun sum -> if negative then sum * -1. else sum
            |> Some
        | _ -> None

    let number (number: string) =
        match tryNumber number with
        | Some number -> number
        | None -> failwith "Input string was no valid number"
