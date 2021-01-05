module Tests

open Expecto

open Dozenalize
open Dozenalize.Types

let config = Config.ab

let tests =
    testList "Group of all tests" [
        testList "Group of tests for number to dozenal string conversion" [
            // Nullcase
            test "double: 0" {
                let result = Display.number config 0.
                Expect.equal result "0" "The strings should equal"
            }
            // Some positive numbers
            test "double: 1" {
                let result = Display.number config 1.
                Expect.equal result "1" "The strings should equal"
            }
            test "double: 12" {
                let result = Display.number config 12.
                Expect.equal result "10" "The strings should equal"
            }
            test "double: 131" {
                let result = Display.number config 131.
                Expect.equal result "AB" "The strings should equal"
            }
            // Some positive numbers with fractions
            test "double: 0.25" {
                let result = Display.number config 0.25
                Expect.equal result "0.3" "The strings should equal"
            }
            test "double: 1.5" {
                let result = Display.number config 1.5
                Expect.equal result "1.6" "The strings should equal"
            }
            test "double: 154.75" {
                let result = Display.number config 154.75
                Expect.equal result "10A.9" "The strings should equal"
            }
            // Some negative numbers
            test "double: -1" {
                let result = Display.number config -1.
                Expect.equal result "-1" "The strings should equal"
            }
            test "double: -12" {
                let result = Display.number config -12.
                Expect.equal result "-10" "The strings should equal"
            }
            test "double: -131" {
                let result = Display.number config -131.
                Expect.equal result "-AB" "The strings should equal"
            }
            // Some negative numbers with fractions
            test "double: -0.25" {
                let result = Display.number config -0.25
                Expect.equal result "-0.3" "The strings should equal"
            }
            test "double: -1.5" {
                let result = Display.number config -1.5
                Expect.equal result "-1.6" "The strings should equal"
            }
            test "double: -154.75" {
                let result = Display.number config -154.75
                Expect.equal result "-10A.9" "The strings should equal"
            }
            // Some different datatypes
            test "decimal: 23" {
                let result = Display.number config 23m
                Expect.equal result "1B" "The strings should equal"
            }
            test "single: 23" {
                let result = Display.number config 23f
                Expect.equal result "1B" "The strings should equal"
            }
            test "bigint: 23" {
                let result = Display.number config 23I
                Expect.equal result "1B" "The strings should equal"
            }
            test "int: 23" {
                let result = Display.number config 23
                Expect.equal result "1B" "The strings should equal"
            }
        ]
    ]
