#load "../.paket/load/net462/Completed/completed.group.fsx"

open Expecto

let addTest =
    testCase "An addition test" <| fun () ->
        let expected = 4
        Expect.equal expected (2+2) "2+2 = 4"

let multTest =
    testCase "A multiplication test" <| fun () ->
        let expected = 4
        Expect.equal expected (2*2) "2*2 = 4"

let intTests =
    testList "Integer math tests" [
        addTest
        multTest
    ]

let config = { FsCheckConfig.defaultConfig with maxTest = 10000 }

let properties =
    testList "FsCheck samples" [
        testProperty "Addition is commutative" <| fun a b ->
            a + b = b + a

        testProperty "Reverse of reverse of a list is the original list" <|
            fun (xs:list<int>) -> List.rev (List.rev xs) = xs

        // you can also override the FsCheck config
        testPropertyWithConfig config "Product is distributive over addition" <|
            fun a b c -> a * (b + c) = a * b + a * c
    ]

let allTests =
    testList "all tests" [
        intTests
        properties
    ]

Tests.runTests defaultConfig allTests
