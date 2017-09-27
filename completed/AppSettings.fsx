#load "../.paket/load/net462/Completed/completed.group.fsx"

#r "System.Configuration.dll"

open System.Configuration

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__

let appSettings = ConfigurationManager.AppSettings
appSettings.["test2"]

printfn "%s" System.Environment.CurrentDirectory

(**
 * Option 1: Use the FSharp.Configuration Type Provider
 *)

open FSharp.Configuration

let [<Literal>] AppSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"
type Settings = FSharp.Configuration.AppSettings<AppSettingsPath>
let [<Literal>] ExecutableFile = __SOURCE_DIRECTORY__ + "/AppSettings.fsx"
Settings.SelectExecutableFile ExecutableFile

Settings.TestUri

(**
 * Option 2: Use System.Xml.Linq 
 *)

#load "Configuration.fsx"
open Configuration

ConfigurationManager.AppSettings.["TestInt"]
ConfigurationManager.ConnectionStrings.["Test1"]
