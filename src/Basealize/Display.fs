namespace Basealize

[<RequireQualifiedAccess>]
module Display =
    let inline number digit numBase precision (number: ^a) =
        let zero: ^a = LanguagePrimitives.GenericZero
        let one: ^a = LanguagePrimitives.GenericOne

        let baseNum: ^a = (Seq.init numBase (fun _ -> one)) |> Seq.sum

        // Helper to convert the front part of the number
        let rec helperFront number parts =
            let rest = (abs number) % numBase

            // Add digit of rest to result
            let parts = int rest |> Digit |> Seq.singleton |> Seq.append parts

            // Calculate what is left for the next run
            let rest' =
                match number with
                | number when number > 0 -> (number - rest) / numBase
                | number -> (number + rest) / numBase

            // If we have nothing left,
            if rest' = 0 then parts else helperFront rest' parts

        // Helper to convert the back part of the number
        let rec helperBack (number: ^a) counter parts =
            let rest = (abs number) * baseNum

            // Add digit of rest to result
            let parts = int rest |> Digit |> Seq.singleton |> Seq.append parts

            // Calculate what is left for the next run
            let rest' = rest % one

            // If we have nothing left
            if rest' = zero then
                parts
            // If we reached precision, we round and quit
            elif counter >= precision then
                if (int) (rest' * baseNum) >= numBase / 2 then
                    // Round up
                    Seq.mapFoldBack
                        (fun part oneUp ->
                            match part with
                            | Separator -> part, oneUp
                            | Digit d ->
                                let d = if oneUp then d + 1 else d
                                let oneUp = d >= numBase
                                d % numBase |> Digit, oneUp)
                        parts
                        true
                    |> fst
                else
                    // Round down, nothing to do
                    parts
            else
                helperBack rest' (counter + 1uy) parts

        // We determine the numbers at the front first
        let front =
            if abs number < one then
                Digit 0 |> Seq.singleton
            else
                helperFront (int number) Seq.empty
                // We have to rev this, the helper works from back to front
                |> Seq.rev

        // Do we have decimal places we need to consider?
        let decimals = abs number % one

        (if decimals > zero && precision > 0uy then
             Seq.append front (Seq.singleton Separator) |> helperBack decimals 1uy
         else
             front)
        |> Seq.map (function
            | Separator -> "."
            | Digit d -> digit d)
        |> String.concat ""
        // Do we need to add a minus?
        |> (fun numberString -> if number < zero then "-" + numberString else numberString)
