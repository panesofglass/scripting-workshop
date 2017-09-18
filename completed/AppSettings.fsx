#r "System.Configuration.dll"
#load "../.paket/load/net462/Completed/completed.group.fsx"

let [<Literal>] appSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"

(**
 * Option 1: Use System.Xml.Linq 
 *)

#r "System.Xml.dll"
#r "System.Xml.Linq.dll"
open System.Xml
open System.Xml.Linq

#if INTERACTIVE
type ConfigurationManager() =
    static let config = XDocument.Load appSettingsPath
    static let section (config:XDocument) name key value =
        query {
            for els in config.Descendants(XName.Get name) do
            for el in els.Descendants(XName.Get "add") do
            let k = el.Attribute(XName.Get key).Value 
            let v = el.Attribute(XName.Get value).Value 
            select (k,v)
        }
        |> dict
    static let appSettings = section config "appSettings" "key" "value"
    static let connectionStrings = section config "connectionStrings" "name" "connectionString"
    static member AppSettings = appSettings
    static member ConnectionStrings = connectionStrings
#endif

ConfigurationManager.AppSettings.["TestInt"]
ConfigurationManager.ConnectionStrings.["Test1"]

(**
 * Option 2: Use the FSharp.Configuration Type Provider
 *)

open FSharp.Configuration

let [<Literal>] appSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"
type Settings = FSharp.Configuration.AppSettings<appSettingsPath>
let [<Literal>] executableFile = __SOURCE_DIRECTORY__ + "/AppSettings.fsx"
Settings.SelectExecutableFile executableFile

Settings.TestUri