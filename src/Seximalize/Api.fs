namespace Seximalize.CSharp

open Basealize.CSharp

open Seximalize

// We redefine Display as a class and translate the inline function into a bunch
// of overloads. We define a static create function, because the interface functions
// are not easily callable otherwise because of the explicit interface definitions
type SeximalPrinter(precision) =
    new() = SeximalPrinter(3uy)

    interface BasePrinter with
        member _.DefaultPrecision = precision

        member _.PrintDigit(digit) = Display.digit digit

        member _.PrintNumber(number: sbyte, precision) = Display.number precision number

        member _.PrintNumber(number: int16, precision) = Display.number precision number

        member _.PrintNumber(number: int, precision) = Display.number precision number

        member _.PrintNumber(number: nativeint, precision) = Display.number precision number

        member _.PrintNumber(number: int64, precision) = Display.number precision number

        member _.PrintNumber(number: single, precision) = Display.number precision number

        member _.PrintNumber(number: double, precision) = Display.number precision number

        member _.PrintNumber(number: decimal, precision) = Display.number precision number

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

    static member Create(precision) =
        SeximalPrinter(precision) :> BasePrinter

    static member Create() = SeximalPrinter() :> BasePrinter

// We redefine Parse as a class.
// We define a static create function, because the interface functions
// are not easily callable otherwise because of the explicit interface definitions
type SeximalParser() =
    interface BaseParser with
        member _.CheckForValidNumber(number) =
            Parse.validNumber number |> Option.isSome

        member _.TryParseDigit(number) =
            Parse.tryDigit number |> Option.toNullable

        member _.ParseDigit(number) = Parse.digit number

        member _.TryParseNumber(number) =
            Parse.tryNumber number |> Option.toNullable

        member _.ParseNumber(number) = Parse.number number

    static member Create() = SeximalParser() :> BaseParser
