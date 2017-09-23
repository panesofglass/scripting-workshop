#load "../.paket/load/net462/Completed/completed.group.fsx"

open Argu

type Arguments =
    | [<Mandatory>] FirstName of string
    | LastName of string
    with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | FirstName _ -> "The requestor's first name"
            | LastName _ -> "The requestor's last name"

let formatName firstName lastName =
    match lastName with
    | Some ln -> sprintf "%s %s" firstName ln
    | None    -> firstName

let args = fsi.CommandLineArgs.[1..]

let argParser = ArgumentParser.Create<Arguments>(errorHandler = ProcessExiter())
let argResults = argParser.Parse(args)
let firstName = argResults.GetResult <@ FirstName @>
let lastName = argResults.TryGetResult <@ LastName @>

(firstName, lastName) ||> formatName |> printfn "Hello, %s!"