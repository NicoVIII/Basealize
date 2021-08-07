namespace Dozenalize

[<AutoOpen>]
module Types =
    type Config = { decChar: string; elChar: string }

[<RequireQualifiedAccess>]
module Config =
    let create dec el = { decChar = dec; elChar = el }

    // Prepared default configs
    let createAb () = create "A" "B"
    let createTe () = create "T" "E"
    let createXe () = create "X" "E"
    let createXz () = create "X" "Z"
    let createGreek1 () = create "δ" "ε"
    let createGreek2 () = create "τ" "ε"
    let createAndrews () = create "X" "ℰ"
    let createKramer () = create "⚹" "#"
    // let dwiggins = // sadly no unicode characters for that :/
    let createPitman () = create "↊" "↋"
