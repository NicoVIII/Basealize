namespace Basealize

[<AutoOpen>]
module DomainTypes =
    type Part =
        | Separator
        | Digit of int
