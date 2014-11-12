type OS =
    | Linux
    | OSX            
    | Windows

let getOS = 
    match int System.Environment.OSVersion.Platform with
    | 4 | 128 -> Linux
    | 6       -> OSX
    | _       -> Windows