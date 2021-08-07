namespace Basealize.CSharp

open System

type BasePrinter =
    abstract DefaultPrecision : byte

    abstract PrintDigit : int -> string

    // Simple call with default precision
    abstract PrintNumber : sbyte -> string
    abstract PrintNumber : int16 -> string
    abstract PrintNumber : int -> string
    abstract PrintNumber : nativeint -> string
    abstract PrintNumber : int64 -> string
    abstract PrintNumber : single -> string
    abstract PrintNumber : float -> string
    abstract PrintNumber : decimal -> string

    // Calls with possibility to overwrite precision
    abstract PrintNumber : sbyte * byte -> string
    abstract PrintNumber : int16 * byte -> string
    abstract PrintNumber : int * byte -> string
    abstract PrintNumber : nativeint * byte -> string
    abstract PrintNumber : int64 * byte -> string
    abstract PrintNumber : single * byte -> string
    abstract PrintNumber : float * byte -> string
    abstract PrintNumber : decimal * byte -> string

type BaseParser =
    abstract CheckForValidNumber : string -> bool
    abstract TryParseDigit : string -> Nullable<float>
    abstract ParseDigit : string -> float
    abstract TryParseNumber : string -> Nullable<float>
    abstract ParseNumber : string -> float
