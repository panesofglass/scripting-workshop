System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

#load "Paket.fsx"
Paket.download()
Paket.restore()

#r "System.Configuration.dll"
#load "../.paket/load/net462/Completed/completed.group.fsx"
#load "Configuration.fsx"
open Configuration

open System
open Argu
open FSharp.Configuration
open FSharp.Data
open XPlot.GoogleCharts


fsi.AddPrinter(fun (chart:XPlot.GoogleCharts.GoogleChart) ->
   chart |> Chart.Show
   "Google Chart")

let [<Literal>] AppSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"
type Settings = FSharp.Configuration.AppSettings<AppSettingsPath>
let [<Literal>] ExecutableFile = __SOURCE_DIRECTORY__ + "/AppSettings.fsx"
Settings.SelectExecutableFile ExecutableFile

// Not always what you want.
//let guidAppKey = Settings.AppKey

let appKey = ConfigurationManager.AppSettings.["AppKey"]

// Extract data
let [<Literal>] ``geocoding.json`` = __SOURCE_DIRECTORY__ + "/geocoding.json"
type Location = JsonProvider<``geocoding.json``>

let extract city =
    let uri = sprintf "https://api.opencagedata.com/geocode/v1/json?q=%s&pretty=1&no_annotations=1&key=%s" city appKey
    Location.Load(uri)

// Transform data
type LocationResult =
  { DisplayName : string
    Latitude : float
    Longitude : float
    City : string
    State : string
    Country : string
    CountryCode : string
    EnergyUse : Runtime.WorldBank.Indicator option }

let transform (source:Location.Root) : LocationResult list =
    let wb = WorldBankData.GetDataContext()
    [ for result in source.Results ->
        let energyUse =
            wb.Countries
            |> Seq.tryFind (fun x -> result.Components.Country.StartsWith(x.Name))
            |> Option.map (fun x -> x.Indicators.``Energy use (kg of oil equivalent per capita)``)
        { DisplayName = result.Formatted
          Latitude = float result.Geometry.Lat
          Longitude = float result.Geometry.Lng
          City = result.Components.City
          State = result.Components.State
          Country = result.Components.Country
          CountryCode = result.Components.Iso31661Alpha2
          EnergyUse = energyUse }
    ]

// Persist data
type LocationSink = CsvProvider<"Location (string),Latitude (float),Longitude (float),City (string),State (string),Country (string),CountryCode (string)">
type EnergyUseSink = CsvProvider<"CountryCode (string),EnergyUse (float)">

let load (data:LocationResult list) =
    let locations, energyUse =
        [ for row in data ->
            let location =
                LocationSink.Row(
                    row.DisplayName,
                    row.Latitude,
                    row.Longitude,
                    row.City,
                    row.State,
                    row.Country,
                    row.CountryCode
                )
            let usage =
                match row.EnergyUse with
                | Some i ->
                    [ for y in 1960..2012 ->
                        EnergyUseSink.Row(row.CountryCode, i.[y]) ]
                | None -> []
            location, usage
        ]
        |> List.unzip
    do
        use locationSink = new LocationSink(locations)
        locationSink.Save("locations.csv")
    do
        let distinct =
            List.concat energyUse
            |> List.distinctBy (fun x -> x.CountryCode)
        use energyUseSink = new EnergyUseSink(distinct)
        energyUseSink.Save("energyUse.csv")

// Chart
let showMap (data:LocationResult list) =
    data
    |> List.map (fun x -> x.Latitude, x.Longitude, x.DisplayName)
    |> Chart.Map
    |> Chart.WithOptions (Options(showTip = true))
    |> Chart.WithHeight 420
    |> Chart.Show

let showEnergyUse (data:LocationResult list) =
    let labels, plots =
        data
        |> List.choose (fun x ->
            match x.EnergyUse with
            | Some energyUse -> Some(x.Country, energyUse)
            | None -> None)
        |> List.distinctBy fst
        |> List.map (fun (country, energyUse) ->
            country, [for y in 1960..2010 -> string y, energyUse.[y]])
        |> List.unzip
    plots
    |> Chart.Line
    |> Chart.WithOptions (Options(title = "Energy use per capita"))
    |> Chart.WithLabels labels
    |> Chart.Show

// Compose
let run city =
    let data = (extract >> transform) city
    load data
    showMap data
    showEnergyUse data

// CLI

type Arguments =
    | City of string
    with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | City _ -> "Enter a location name"

let args = fsi.CommandLineArgs.[1..]
//let args = [|"Map.fsx";"--city";"Adelaide"|].[1..]
let argParser = ArgumentParser.Create<Arguments>(errorHandler = ProcessExiter())
let argResults = argParser.Parse(args)
let city = argResults.GetResult <@ City @>
run city
