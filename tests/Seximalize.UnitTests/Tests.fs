module Tests

open Expecto

open Seximalize

[<AutoOpen>]
// Contains some helper functions to simplify testing
module Helper =
    let inline display x = Display.number 10uy x
    let inline displayPrecision precision x = Display.number (byte precision) x

    let testForNumber (nr: double) nrString =
        test $"double: {nr}" {
            let result = display nr
            Expect.equal result nrString "The strings should equal"
        }

    let testForString nrString (nr: double) =
        test $"string: {nrString}" {
            let result = Parse.number nrString
            Expect.equal result nr "The numbers should equal"
        }

    let testWithPrecision precision (nr: double) nrString =
        test $"double: {nr} (precision {precision})" {
            let result = displayPrecision precision nr
            Expect.equal result nrString "The strings should equal"
        }

[<AutoOpen>]
// Contains test data
module Data =
    // Some numbers, which are tested as they are and negated
    let numberTestData =
        [
            1., "1"
            131., "335"
            // With fractions
            0.25, "0.13"
            154.75, "414.43"
        ]
        // Add negated versions
        |> List.fold
            (fun newList (nr, nrString) ->
                (nr * -1., "-" + nrString)
                :: (nr, nrString) :: newList)
            []

    let precisionTestData =
        [
            0.2, 0, "0"
            0.2, 1, "0.1"
            0.2, 10, "0.1111111111"
            0.99, 1, "1.0"
            0.999999, 5, "1.00000"
        ]

let tests =
    testList
        "Tests"
        [
            testList
                "For number to dozenal string conversion"
                [
                    // Null case
                    testForNumber 0. "0"

                    // Test some numbers
                    for (nr, nrString) in numberTestData do
                        testForNumber nr nrString

                    // Test some precisions
                    for (nr, precision, nrString) in precisionTestData do
                        testWithPrecision precision nr nrString

                    // Some different datatypes
                    test "decimal: 23" {
                        let result = display 23m
                        Expect.equal result "35" "The strings should equal"
                    }
                    test "single: 23" {
                        let result = display 23f
                        Expect.equal result "35" "The strings should equal"
                    }
                    test "bigint: 23" {
                        let result = display 23I
                        Expect.equal result "35" "The strings should equal"
                    }
                    // Uints do not work, because of the use of abs
                    (*test "uint: 23" {
                  let result = display 23u
                  Expect.equal result "1B" "The strings should equal"
              }*)
                    test "int: 23" {
                        let result = display 23
                        Expect.equal result "35" "The strings should equal"
                    }
                ]
            testList
                "For dozenal string to number conversion"
                [
                    // Null case
                    testForNumber 0. "0"

                    // Test some numbers
                    for (nr, nrString) in numberTestData do
                        testForString nrString nr
                ]
            // Test property display >> parse = id
            testProperty "property: display >> parse = id"
            <| fun number ->
                let displayed = display number
                let parsed = Parse.number displayed |> int
                parsed = number
        ]
