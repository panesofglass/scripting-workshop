#load "../.paket/load/net462/Completed/completed.group.fsx"

(**
 * Option 1: Use System.Xml.Linq 
 *)

#load "Configuration.fsx"
open Configuration

ConfigurationManager.AppSettings.["TestInt"]
ConfigurationManager.ConnectionStrings.["Test1"]

(**
 * Option 2: Use the FSharp.Configuration Type Provider
 *)

#r "System.Configuration.dll"
open FSharp.Configuration

let [<Literal>] AppSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"
type Settings = FSharp.Configuration.AppSettings<AppSettingsPath>
let [<Literal>] ExecutableFile = __SOURCE_DIRECTORY__ + "/AppSettings.fsx"
Settings.SelectExecutableFile ExecutableFile

Settings.TestUri