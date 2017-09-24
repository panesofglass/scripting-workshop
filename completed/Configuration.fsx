#r "System.Xml.dll"
#r "System.Xml.Linq.dll"
open System.Xml
open System.Xml.Linq

let [<Literal>] AppSettingsPath = __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"

type ConfigurationManager() =
    static let config = XDocument.Load AppSettingsPath
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
