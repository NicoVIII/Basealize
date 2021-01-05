namespace Dozenalize

open Dozenalize.Types

[<RequireQualifiedAccess>]
module Display =
    let digit config digit =
        match digit with
        | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 -> string digit
        | 10 -> config.decChar
        | 11 -> config.elChar
        | _ -> failwithf "%i is not a valid dozenal digit." digit

    let inline number config (number:^a) =
        let zero:^a = LanguagePrimitives.GenericZero
        let one:^a = LanguagePrimitives.GenericOne
        let twelve:^a = (Seq.init 12 (fun _ -> one)) |> Seq.sum

        // Helper to convert the front part of the number
        let rec helperFront config number parts =
            let rest = (abs number) % 12

            // Add digit of rest to result
            let parts =
                int rest
                |> digit config
                |> Seq.singleton
                |> Seq.append parts

            // Calculate what is left for the next run
            let rest' =
                match number with
                | number when number > 0 -> (number - rest) / 12
                | number -> (number + rest) / 12

            // If we have nothing left,
            if rest' = 0 then
                parts
            else
                helperFront config rest' parts

        // Helper to convert the back part of the number
        let rec helperBack config (number:^a) parts =
            let rest = (abs number) * twelve

            // Add digit of rest to result
            let parts =
                int rest
                |> digit config
                |> Seq.singleton
                |> Seq.append parts

            // Calculate what is left for the next run
            let rest' = rest % one

            // If we have nothing left,
            if rest' = zero then
                parts
            else
                helperBack config rest' parts

        // We determine the numbers at the front first
        let front =
            if abs number < one then
                "0" |> Seq.singleton
            else
                helperFront config (int number) Seq.empty
                // We have to rev this, the helper works from back to front
                |> Seq.rev

        // Do we have decimal places we need to consider?
        let decimals = abs number % one
        if decimals > zero then
            Seq.append front (Seq.singleton ".")
            |> helperBack config decimals
        else
            front
        |> String.concat ""
        // Do we need to add a minus?
        |> (fun numberString -> if number < zero then "-" + numberString else numberString)
