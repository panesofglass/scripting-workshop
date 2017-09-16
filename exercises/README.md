# The Agony and the Ecstasy of F# Scripting

## Notes

https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/fsharp-interactive/

- Familiary with F# Interactive
    - Creating a script file in VS, VS Code, etc. = .fsx
    - Running fsi/fsharpi from cli
    - Using `;;` to end lines in fsi
    - Quitting session with `#quit`
    - Resetting interactive session
    - Understanding `it`
    - printing and formatting results
    - timing with `#time`
    - loading scripts with `#load`
    - referencing assemblies with `#r`

- Challenges
    - Referencing NuGet packages
        - [Paket to the rescue](https://fsprojects.github.io/Paket/paket-generate-load-scripts.html)
        - Referencing files/scripts
            - [git](https://fsprojects.github.io/Paket/git-dependencies.html)
            - [github](https://fsprojects.github.io/Paket/github-dependencies.html)
            - [http](https://fsprojects.github.io/Paket/http-dependencies.html)
                - FsSnip
                - Any script available via HTTP
    - Relative paths
        - What's the current directory? Not your current working directory?
        - Understanding `__SOURCE_DIRECTORY__`
        - Set current working directory using `System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__`
    - Config files
        - Cannot leverage *.config files
        - Parsing config files to retrieve values
        - Setting config values in AppDomain

``` fsharp
let [<Literal>] connectionString =
    "Data Source=.;Initial Catalog=Todo;Integrated Security=SSPI"
let [<Literal>] key = "ConnectionString"
AppDomain.CurrentDomain.SetData(key, connectionString)
let connString = lazy AppDomain.CurrentDomain.GetData(key)
```

- F# Interactive Advanced Features
    - [fsi.exe CLI arguments]( https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/fsharp-interactive/fsharp-interactive-options)
    - Using fsiAnyCPU in VS
    - Using x64 fsi(AnyCPU)