#load "Process.fsx"

open System.IO

let paketWorkingDirectory =
    let dir = DirectoryInfo(__SOURCE_DIRECTORY__)
    dir.Parent.FullName

let full path = Path.Combine(paketWorkingDirectory, path)

let download () =
    if not(File.Exists(full ".paket/paket.exe")) then
        let proc =
            Process.Shell.Exec(
                fileName = full ".paket/paket.bootstrapper.exe",
                workingDirectory = paketWorkingDirectory)
        proc.WaitForExit()
        if proc.ExitCode <> 0 then failwith "paket.bootstrapper.exe failed to download paket.exe"
    else ()

let restore () =
    let proc =
        Process.Shell.Exec(
            fileName = full ".paket/paket.exe",
            arguments = "restore",
            workingDirectory = paketWorkingDirectory)
    proc.WaitForExit()
    if proc.ExitCode <> 0 then failwith "paket.exe failed to restore packages"

let generateLoadScripts framework =
    let proc =
        Process.Shell.Exec(
            fileName = full ".paket/paket.exe",
            arguments = sprintf "generate-load-scripts --framework %s" framework,
            workingDirectory = paketWorkingDirectory)
    proc.WaitForExit()
    if proc.ExitCode <> 0 then failwithf "paket.exe failed to generate load scripts for %s" framework
