namespace Dozenalize.CSharp

open Basealize.CSharp

open Dozenalize

// We define a class with static methods to create configs
// This is simply done because for C# PascalCase is more prominent than camelCase
type Config private () =
    static member Create(dec: string, el: string) = Config.create dec el
    static member CreateAb() = Config.createAb ()
    static member CreateTe() = Config.createTe ()
    static member CreateXe() = Config.createXe ()
    static member CreateXz() = Config.createXz ()
    static member CreateGreek1() = Config.createGreek1 ()
    static member CreateGreek2() = Config.createGreek2 ()
    static member CreateAndrews() = Config.createAndrews ()
    static member CreateKramer() = Config.createKramer ()
    static member CreatePitman() = Config.createPitman ()

// We redefine Display as a class and translate the inline function into a bunch
// of overloads. We define a static create function, because the interface functions
// are not easily callable otherwise because of the explicit interface definitions
type DozenalPrinter(config, precision) =
    new(config) = DozenalPrinter(config, 3uy)

    member _.Config = config

    interface BasePrinter with
        member _.DefaultPrecision = precision

        member this.PrintDigit(digit) = Display.digit this.Config digit

        member this.PrintNumber(number: sbyte, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: int16, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: int, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: nativeint, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: int64, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: single, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: double, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: decimal, precision) =
            Display.number this.Config precision number

        member this.PrintNumber(number: sbyte) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: int16) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: int) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: nativeint) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: int64) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: single) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: double) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

        member this.PrintNumber(number: decimal) =
            let this = this :> BasePrinter
            this.PrintNumber(number, this.DefaultPrecision)

    static member Create(config, precision) =
        DozenalPrinter(config, precision) :> BasePrinter

    static member Create(config) = DozenalPrinter(config) :> BasePrinter

// We redefine Parse as a class.
// We define a static create function, because the interface functions
// are not easily callable otherwise because of the explicit interface definitions
type DozenalParser(config) =
    member _.Config = config

    interface BaseParser with
        member this.CheckForValidNumber(number) =
            Parse.validNumber this.Config number |> Option.isSome

        member this.TryParseDigit(number) =
            Parse.tryDigit this.Config number |> Option.toNullable

        member this.ParseDigit(number) = Parse.digit this.Config number

        member this.TryParseNumber(number) =
            Parse.tryNumber this.Config number |> Option.toNullable

        member this.ParseNumber(number) = Parse.number this.Config number

    static member Create(config) = DozenalParser(config) :> BaseParser
