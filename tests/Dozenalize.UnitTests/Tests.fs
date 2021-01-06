module Tests

open Expecto

open Dozenalize
open Dozenalize.Types

let config = Config.PreConf.ab
let inline display x = Display.number config 10uy x

let inline displayPrecision precision x =
    Display.number config (byte precision) x

let parse = Parse.number config

let tests =
    testList
        "Tests"
        [ testList
            "For number to dozenal string conversion"
            [
              // Nullcase
              test "double: 0" {
                  let result = display 0.
                  Expect.equal result "0" "The strings should equal"
              }
              // Some positive numbers
              test "double: 1" {
                  let result = display 1.
                  Expect.equal result "1" "The strings should equal"
              }
              test "double: 131" {
                  let result = display 131.
                  Expect.equal result "AB" "The strings should equal"
              }
              // Some positive numbers with fractions
              test "double: 0.25" {
                  let result = display 0.25
                  Expect.equal result "0.3" "The strings should equal"
              }
              test "double: 154.75" {
                  let result = display 154.75
                  Expect.equal result "10A.9" "The strings should equal"
              }
              // Some negative numbers
              test "double: -1" {
                  let result = display -1.
                  Expect.equal result "-1" "The strings should equal"
              }
              test "double: -131" {
                  let result = display -131.
                  Expect.equal result "-AB" "The strings should equal"
              }
              // Some negative numbers with fractions
              test "double: -0.25" {
                  let result = display -0.25
                  Expect.equal result "-0.3" "The strings should equal"
              }
              test "double: -154.75" {
                  let result = display -154.75
                  Expect.equal result "-10A.9" "The strings should equal"
              }
              // Test some precisions
              test "double: 0.2 (precision 0)" {
                  let result = displayPrecision 0 0.2
                  Expect.equal result "0" "The strings should equal"
              }
              test "double: 0.2 (precision 1)" {
                  let result = displayPrecision 1 0.2
                  Expect.equal result "0.2" "The strings should equal"
              }
              test "double: 0.2 (precision 10)" {
                  let result = displayPrecision 10 0.2
                  // Should round up
                  Expect.equal result "0.2497249725" "The strings should equal"
              }
              test "double: 0.99 (precision 1)" {
                  let result = displayPrecision 1 0.99
                  // Should round up
                  Expect.equal result "1.0" "The strings should equal"
              }
              // Some different datatypes
              test "decimal: 23" {
                  let result = display 23m
                  Expect.equal result "1B" "The strings should equal"
              }
              test "single: 23" {
                  let result = display 23f
                  Expect.equal result "1B" "The strings should equal"
              }
              test "bigint: 23" {
                  let result = display 23I
                  Expect.equal result "1B" "The strings should equal"
              }
              // Uints do not work, because of the use of abs
              (*test "uint: 23" {
                  let result = display 23u
                  Expect.equal result "1B" "The strings should equal"
              }*)
              test "int: 23" {
                  let result = display 23
                  Expect.equal result "1B" "The strings should equal"
              } ]
          testList
              "For dozenal string to number conversion"
              [
                // Nullcase
                test "string: 0" {
                    let result = Parse.number config "0"
                    Expect.equal result 0. "The numbers should equal"
                }
                // Some positive numbers
                test "string: 1" {
                    let result = Parse.number config "1"
                    Expect.equal result 1. "The numbers should equal"
                }
                test "string: AB" {
                    let result = Parse.number config "AB"
                    Expect.equal result 131. "The numbers should equal"
                }
                // Some positive numbers with fractions
                test "string: 0.3" {
                    let result = Parse.number config "0.3"
                    Expect.equal result 0.25 "The numbers should equal"
                }
                test "string: 10A.9" {
                    let result = Parse.number config "10A.9"
                    Expect.equal result 154.75 "The numbers should equal"
                }
                // Some negative numbers
                test "string: -1" {
                    let result = Parse.number config "-1"
                    Expect.equal result -1. "The numbers should equal"
                }
                test "string: -AB" {
                    let result = Parse.number config "-AB"
                    Expect.equal result -131. "The numbers should equal"
                }
                // Some negative numbers with fractions
                test "string: -0.3" {
                    let result = Parse.number config "-0.3"
                    Expect.equal result -0.25 "The numbers should equal"
                }
                test "string: -10A.9" {
                    let result = Parse.number config "-10A.9"
                    Expect.equal result -154.75 "The numbers should equal"
                } ]
          testList
              "properties for conversion back and forth"
              [
                // Test property display >> parse = id
                testProperty "display >> parse = id"
                <| fun number ->
                    let displayed = display number
                    let parsed = Parse.number config displayed |> int
                    parsed = number ] ]
