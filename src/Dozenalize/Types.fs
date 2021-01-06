namespace Dozenalize

open System

module Types =
    type Config =
        { decChar: string
          elChar: string
          precision: byte }

    module Config =
        let create dec el precision =
            { decChar = dec
              elChar = el
              precision = precision }

        module PreConf =
            let private defaultPrecision = 10uy

            let ab = create "A" "B" defaultPrecision
            let te = create "T" "E" defaultPrecision
            let xe = create "X" "E" defaultPrecision
            let xz = create "X" "Z" defaultPrecision
            let greek1 = create "δ" "ε" defaultPrecision
            let greek2 = create "τ" "ε" defaultPrecision
            let andrews = create "X" "ℰ" defaultPrecision
            let kramer = create "⚹" "#" defaultPrecision
            // let dwiggins = // sadly no unicode characters for that :/
            let pitman = create "↊" "↋" defaultPrecision
