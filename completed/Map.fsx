#load "../.paket/load/net462/Completed/completed.group.fsx"
#load "Configuration.fsx"
open Configuration

open System
open FSharp.Data
open XPlot.GoogleCharts

Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

fsi.AddPrinter(fun (chart:XPlot.GoogleCharts.GoogleChart) ->
   chart |> Chart.Show
   "Google Chart")

let appKey = ConfigurationManager.AppSettings.["AppKey"]
let request city = sprintf "https://api.opencagedata.com/geocode/v1/json?q=%s&pretty=1&no_annotations=1&key=%s" city appKey
let [<Literal>] ``geocoding.json`` = __SOURCE_DIRECTORY__ + "/geocoding.json"
type Location = JsonProvider<``geocoding.json``>

let sf = Location.Load(request "San Francisco")
let data =
    [ for result in sf.Results ->
        float result.Geometry.Lat, float result.Geometry.Lng, result.Formatted ]
let options = Options(showTip = true)
data
|> Chart.Map
|> Chart.WithOptions options
|> Chart.WithHeight 420
