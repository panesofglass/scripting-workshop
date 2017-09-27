#load "../.paket/load/net461/Completed/completed.group.fsx"

open XPlot.GoogleCharts

[1..10] |> Chart.Line |> Chart.Show

fsi.AddPrinter(fun (chart:XPlot.GoogleCharts.GoogleChart) ->
   chart |> Chart.Show
   "Google Chart")

[ 1 .. 10 ] |> Chart.Line

open FSharp.Data

let wb = WorldBankData.GetDataContext()
let cz = wb.Countries.``Czech Republic``.Indicators
let eu = wb.Countries.``European Union``.Indicators
let czschool = cz.``School enrollment, tertiary (gross), gender parity index (GPI)``
let euschool = eu.``School enrollment, tertiary (gross), gender parity index (GPI)``

[czschool; euschool]
|> Chart.Line
|> Chart.WithLabels ["CZ"; "EU"]

let label (xs:Runtime.WorldBank.Indicator) =
    [ for y in 1985..2012 -> string y, xs.[y] ]

[label czschool; label euschool]
|> Chart.Line
|> Chart.WithLabels ["CZ"; "EU"]
