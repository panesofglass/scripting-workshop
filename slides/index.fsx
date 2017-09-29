(**
- title : The Agony and the Ecstasy of F# Scripting
- description : Discover the wonderful world of F# scripting, including its agonies and ecstasies. We'll work through building a simple application starting with a script and moving to the relevant library and test projects and discuss strategies for managing app settings and connection strings, as well as how to access command line arguments.
- author : Ryan Riley
- theme : night
- transition : default

***
*)

(*** hide ***)
open System
let originalCwd = System.Environment.CurrentDirectory
let parentDir =
    let dir = System.IO.DirectoryInfo(__SOURCE_DIRECTORY__)
    dir.Parent.FullName
let setCwd dir =
    System.Environment.CurrentDirectory <- System.IO.Path.Combine(parentDir, dir)

(**

## The Agony and the Ecstasy of F# Scripting

[![The Agony and the Ecstasy](images/the-agony-and-the-ecstasy.jpg)](https://emerdelac.files.wordpress.com/2012/05/the-agony-and-the-ecstasy.jpg)

' Welcome to The Agony and the Ecstasy of F# Scripting workshop.
' This workshop targets beginners but should be informative to those
' with more experience.
' I chose the title based on the movie of a similar name. Are any of you
' familiar with it? The Agony and the Ecstasy covers Michelangelo's painting
' the Cistene Chapel, a beautiful work of art. Scripting can feel like a
' beautful work of art, and it offers a terrific workflow for building programs.
' However it comes with its own, unique set of challenges, and scripts
' have a tendency to fall behind and then get discarded, especially when
' they present frustrating errors.

***

## Overview

' The goal for this workshop is for you to learn how to leverage F#'s
' scripting capabilities. F# scripting is incredibly useful; however,
' it has some frustrating rough edges.
' In this workshop, we will build a simple ETL application. Along the
' way, I'll highlight the challenging areas and provide solutions for
' overcoming them.

---

### ETL Applications

' You may wonder why I would choose an ETL application as a workshop
' project. It's a good question. It's exciting to build web server
' applications, especially those with beautiful front-end UIs.
' However, we have a limited time, and in doing this, we would not
' have time enough to cover as many issues peculiar to scripting.
' ETL programs are simple, highlight some of the benefits of using
' F# practically, provide a way of introducing F# into any work
' environment, and lend themselves well to scripting. If you are
' currently using SSIS for ETL projects, you may find an F# replacement
' makes it much easier to parallelize, debug, and deploy.

***

## What's not covered?

' We won't have time to cover every aspect of scripting in our two hour
' workshop today. 

---

### Working with Databases

' We'll be covering data access, but we won't be covering SQL databases
' in particular. While this is a very common use case, it often requires
' more setup time than we have available.
' However, we will cover accessing config files, which are typically one
' of the more challenging areas of working with databases from scripts.

---

### Fable

' Fable, the F# to JavaScript compiler, is definitely worth your time
' to investigate and expands the possiblities of F# scripting since it
' opens F# to the entire JavaScript ecosystem.

---

### Notebooks

' Jupyter Notebooks, available both locally and in Azure, provide a
' fantastic opportunity for leveraging scripts. The F# integration
' provides several helpers to alleviate some of the pain points
' we'll work through today.

---

### Azure Functions, AWS Lambda, etc.

' One reason to learn more about F# scripts is to leverage them as
' deployable applications in services like Azure Functions, AWS Lambda,
' etc.
' All of these are fantastic topics, and if time permits, we will look
' through a few examples.

***

## What we will cover

* Creating and running scripts
* Iterating design
* Loading and referencing files
* Referencing NuGet packages
* Accessing values from .config files
* Working with data
* Leveraging scripts for <acronym title="Extract-Transform-Load">ETL</acronym>
* Testing
* Visualizing data sets
* Accessing command line parameters
* Using scripts in a project
* Debugging scripts
* Build and deploy scripting with FAKE 

***

## Getting Started

![ecstasy](images/ecstasy.png)

' In order to begin, you must have F# installed. I assume you have installed
' it already, but if not, please go to http://fsharp.org/ to find the instructions
' for your environment, or ask for help as we begin the first exercise.
' F# Interactive can be run from the command line or from several IDEs and
' text editors. To begin, please find the correct command line executable
' for your environment. We are going to start simply using the CLI tool for now.

---

### Finding FSI on Windows

`C:\Program Files (x86)\Microsoft SDKs\F#\4.1\Framework\v4.0\fsiAnyCPU.exe`

' F# Interactive is hidden in the Microsoft SDKs folder on Windows and not
' added to the `PATH` by default. It is worth the extra effort to add this
' to your path if you use it regularly.

---

![agony](images/agony.png)

---

### Finding FSI on Mac and Linux

`fsharpi`

' Mono adds the alias `fsharpi`. If you run `which fsharpi`, you'll likely
' find it is in a folder in your path, so it's readily accessible.

---

![ecstasy](images/ecstasy.png)

---

### F# Interactive in the Terminal

    F# Interactive for F# 4.1
    Freely distributed under the Apache 2.0 Open Source License

    For help type #help;;

    > 

' Please try running the executable on your command line.
' You should see a prompt. F# interactive will let you enter F# code
' and run it immediately. This can be useful for very simple use cases.

---

### Enter `2 + 2;;`

    > 2 + 2;;
    val it : int = 4

    > 

' You should see the output appear immediately underneath.
' The result is assigned to a value `it`, which is typed as an `int`.
' Note the `;;` at the end. That tells F# Interactive that you are
' done entering the expression, and you want it evaluated.

---

### Enter `it;;`

    > it;;
    val it : int = 4

    > 

' You can get F# Interactive to repeat the value by entering `it;;`.
' This is useful when working directly within the F# Interactive prompt,
' as you can enter code without assigning it to a name and just use `it`
' when you want to use the result.

---

### Enter `let x = it;;`

    > let x = it;;
    val x : int = 4

    > 

' You can also write assignments directly within F# Interactive. 
' We can assign functions in the same way.
' Note that the value has been assigned to the specified name `x`,
' not `it`. However, you can still access `it` as the F# Interactive
' session retains everything loaded during the session until you
' restart or create a new session.

---

### Enter `type MyClass() = member val Name = "" with get, set;;`

    > type MyClass() =                      
    -     member val Name = "" with get, set;;
    type MyClass =
      class
        new : unit -> MyClass
        member Name : string
        member Name : string with set
      end

    > 

' We can also enter type definitions. Here I've defined a class, but this
' could just as easily be a record, discriminated union, etc.
' In this case, we have a multiline expression, and the `;;` is found
' at the end of the second line.

---

### Enter `quit;;`

    > #quit;;

    - Exit...

' You can exit the F# Interactive prompt with the `#quit;;` command.

***

## Creating and Running Scripts

' While you can use F# Interactive directly, you'll find it is far
' more effective to work with script files. F# Interactive was designed
' more as an execution environment than an editing environment and therefore
' provides no syntax highlighting or code completion, at least in most
' environments.

---

### Open a Text Editor

    $ cd ~/scripting-workshop/exercises
    $ code Script.fsx

' Let's create a script file with extension `.fsx`.
' Open your favorite IDE or text editor. I'll be using VS Code with Ionide,
' but you can follow along just as well with Visual Studio, Visual Studio for Mac,
' Atom with Ionide, Emacs, Vim, etc. Most editors have an F# plugin
' to provide syntax highlighting at a minimum, as well as F# Interactive integration.

---

### Enter the following code snippets:

*)

(*** define: my-class ***)
type MyClass() =
    member val Name = "" with get, set
    member this.Sum(x, y) = x + y
(*** define: script ***)
let x = MyClass(Name = "Some Name")
let result = x.Sum(2, 2)

(*** include: my-class ***)
(*** include: script ***)
(*** define-output: my-class-result ***)
printfn "%s: %i" x.Name result

(**

' This code is nothing special. It's merely a snippet to give you
' more than just `2 + 2`. Feel free to write any other F# code you
' like if this snippet gives you the heebie-jeebies.
' We are artists, after all.

---

### Run the Script

    $ fsharpi Script.fsx

*)

(*** include-output: my-class-result ***)

(**

' You can run the script in several ways.
' You can run it from the command line, as I show above.
' We can also run from the editor, in most cases.
' I have the script opened in Visual Studio Code.
' I can run the script by pressing Alt-Enter or by
' going to the VS Code commands and running the script.
' This same approach should work with any editor you are using,
' though the commands to run the script may differ.

---

![ecstasy](images/ecstasy.png)

' That wasn't so bad. Now you know how to create and run scripts.
' You most likely already knew at least this much, which is why
' we are quickly moving on ...

***

## Loading and Referencing Files

' It's unlikely you'll have many uses for a script as isolated as the
' one above. It's also unwise to try to maintain scripts exceeding
' hundreds or thousands of lines. At some point, you'll want to break
' your script up or reference files or assemblies to provide more
' functionality.

---

### Finding FSI

* Windows: C:\Program Files (x86)\Microsoft SDKs\F#\<version>\Framework\<version>\
* e.g. C:\Program Files (x86)\Microsoft SDKs\F#\4.1\Framework\v4.0\fsiAnyCPU.exe
* Mac/Linux: fsharpi (via mono)

---

### References

*)

(*** define: sys-config-ref ***)
#r "System.Configuration.dll"

(*** include: sys-config-ref ***)

(**

' You reference assemblies with the `#r` directive, followed by the
' path to the assembly. For assemblies stored in the Global Assembly
' Cache, or GAC, you can simply reference the assembly filename.
' Make sure to include the `.dll` suffix, as this needs to be the actual
' filename.
' Note: `#r` uses the script's directory as the relative path to the reference.

---

### CLI References

    $ fsharpi --reference:System.Configuration.dll

    F# Interactive for F# 4.1
    Freely distributed under the Apache 2.0 Open Source License

    For help type #help;;

    > open System.Configuration;;
    > ConfigurationManager.AppSettings;;    
    val it : System.Collections.Specialized.NameValueCollection = seq []

    > 

' You can also add references on the command line. This is useful if you
' a) always want a specific set of references available when running FSI
' or b) want to avoid adding `#r` directives in your scripts.
' Another possibility is to use this and the `no-framework` directive
' to override the version of `FSharp.Core` loaded in Visual Studio 2015's
' F# Interactive. However, this is beyond the scope of this workshop.

---

![ecstasy](images/ecstasy.png)

' Easy peasy

---

### Loading Files

Create a new file, `Library.fs`.

' You reference assemblies, and you load files. You can load either
' `.fsi`, `.fs`, or `.fsx` files. Let's add a library file and move
' our `type` definition into it.

---

### Files so far

    - scripting-workshop
      - exercises
        - Library.fs
        - Script.fsx

---

### Library.fs

*)

(*** include: my-class ***)

(**

---

### Script.fsx

*)

(*** include: sys-config-ref ***)
(*** include: script ***)

(**

' This is the state of our script so far. However, there's a problem.
' We no longer have access to the `MyClass` type.

---

### Script.fsx

*)

(*** define: library-load ***)
#load "Library.fs"

(*** include: sys-config-ref ***)
(*** include: library-load ***)
(*** include: script ***)

(**

' We can fix this using the `#load` directive.

---

### Run Script.fsx

' Now, run the script again.

---

    $ fsharpi Script.fsx
    /Script.fsx(3,9): error FS0039: The value or constructor 'MyClass' is not defined.

---

![agony](images/agony.png)

---

### What went wrong?

' If you are using an editor like VS or VS Code with Ionide,
' your script likely shows red error squiggles. The problem
' is that there's no namespace provided in `Library.fs` yet,
' so F# Interactive assumes a namespace based on the filename.

---

### Script.fsx

*)

(*** include: sys-config-ref ***)
(*** include: library-load ***)
open Library
(*** include: script ***)

(**

' By opening the `Library` namespace, we can get the script to run properly.
' Alternatively, you could provide a namespace in `Library.fs`, which is
' more practical since you'll want that if you plan to use it in a library
' or executable.

---

![ecstasy](images/ecstasy.png)

***

## Iterating Design

![ecstasy](images/ecstasy.png)

' Incidentally, this is a very common workflow when designing an application's types.
' You write some code, test it out, modify, then move it into a source file once you
' like the design. You can leave some of the exercise code in the script for later
' iteration or evaluation.
' Design applies to both domain modeling and data modeling.
' For domain modeling, you may want to use scripts to understand
' the ramifications of certain design decisions. For example,
' is a record the right type, or might you need a discriminated
' union to better cover multiple cases, especially if you may need
' more in the future. How might units of measure help or hinder?
' With respect to data modeling, it can be helpful to throw the
' data in a plot. Interactive sessions work well for this, and you
' can check calculations more easily with visualizations.

***

## Referencing NuGet Packages

' We now know how to reference assmblies available on the local machine,
' as well as files. But what about NuGet packages?

---

### `nuget.exe`

    #r "../packages/Argu.3.7.0/lib/net40/Argu.dll" 
    #r "../packages/FSharp.Data.2.3.3/lib/net40/FSharp.Data.dll" 

' If you are using NuGet directly, you will find yourself in a world of pain
' and anguish. NuGet installs packages with the version as part of the file
' path by default. While this is helpful if you want multiple versions, it's
' unlikely that is the case. Sure, you can tediously hand-write all the references
' in only a little bit of time, but what happens once you need to update packages?
' In addition, you must also reference any of the package dependencies of each
' referenced package. As you can imagine, this can become quite daunting.

---

![agony](images/agony.png)

---

### [Paket](https://fsprojects.github.io/Paket)

* No versions in paths
* Consistent, locked versions
* Dependency groups
* `generate-load-scripts`

' Paket provides a terrific alternative to NuGet. By default, it _does not_
' include paths in versions making your references less prone to breaking.
' It maintains consistent versions throughout your project, so you can't
' accidentally reference different versions of the same dependency.
' You _can_ group dependencies so that you may use different versions of
' a dependency in different contexts. Lastly, and best of all, it provides
' a `generate-load-scripts` command with which you can easily create a single
' load script for all of your paket dependencies.
' Some editors provide utilties for generating package load scripts, but
' the paket command allows you to switch editors at will without worrying
' about different behaviors across editors. This is especially helpful in a
' team setting.

---

![ecstasy](images/ecstasy.png)

---

### Add Packages

' Open the `paket.dependencies` file at the root of the `scripting-workshop`
' folder and add the following dependenices, which we will use throughout
' the rest of the workshop:

    nuget Argu
    nuget Expecto
    nuget Expecto.BenchmarkDotNet
    nuget Expecto.FsCheck
    nuget FSharp.Configuration
    nuget FSharp.Data
    nuget XPlot.GoogleCharts

---

### Generate Load Scripts

    ./.paket/paket.exe generate-load-scripts --group main --type fsx --framework net461

' Run the paket command to generate fsx load scripts for group main and
' framework net461. This will generate the references necessary for our
' use and no more. Note that you can specify different groups and types,
' the other type being csx for C# Interactive.
' NOTE: if you are on Mac or Linux, you'll need to prefix the above with `mono`.

---

### Load Dependencies

*)

#load "../.paket/load/net461/main.group.fsx"

(*** hide ***)
#load "../.paket/load/net461/Completed/completed.group.fsx"

(**

' We can now reference all of our references in one fell swoop!

---

![ecstasy](images/ecstasy.png)

***

## Accessing Values from .config Files

' We are about to switch to accessing a data set. However, the data set
' we want requires an application key. Where do you typically store such
' secrets? In a config file, of course!

---

### Add an `App.config` file

    <?xml version="1.0" encoding="utf-8"?>
    <configuration>
      <appSettings>
        <add key="AppKey" value="7B7EB384FEBA4409B56066FF63F1E8D0" />
        <add key="test2" value="Some Test Value 5" />
        <add key="TestInt" value="102" />
        <add key="TestBool" value="True" />
        <add key="TestDouble" value="10.01" />
        <add key="TestTimeSpan" value="2.01:02:03.444" />
        <add key="TestDateTime" value="02/01/2014 03:04:05.777" />
        <add key="TestUri" value="http://fsharp.org" />
        <add key="TestGuid" value="{7B7EB384-FEBA-4409-B560-66FF63F1E8D0}" />
      </appSettings>
      <connectionStrings>
        <add name="Test1"
          connectionString="Server=.;Database=myDataBase;Integrated Security=True;" />
        <add name="Test2"
          connectionString="Server=.;Database=myDataBase2;Integrated Security=True;" />
      </connectionStrings>
    </configuration>

---

### Create `AppSettings.fsx`

*)

(*** include: sys-config-ref ***)

open System.Configuration

ConfigurationManager.AppSettings.["test2"]

(*** hide ***)
let appSettingsTest2 = ConfigurationManager.AppSettings.["test2"]

(*** include-value: appSettingsTest2 ***)

(**

' Create an AppSettings.fsx script to get a clean slate.
' Add teh following code to retrieve the `test2` setting.

---

![agony](images/agony.png)

---

### What went wrong?

---

### Problem 1: CurrentDirectory

*)

System.Environment.CurrentDirectory

(*** include-value: originalCwd ***)

(**
 
' The first problem is that the script is not necessarily running it its own folder.
' The current directory returned is the directory in which F# Interactive is running.
' Depending on where and how you run your script, you may not be able to access
' the config file you want.
 
---

### Change the Current Directory

*)

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
System.Environment.CurrentDirectory

(*** include-value:System.Environment.CurrentDirectory ***)

(**

' We can set the current directory using this line of code.
' You'll typically want to work from your source directory.
' Try adding this line just after opening `System.Configuration`,
' restart your environment, and try again.

---

*)

(*** include-value: appSettingsTest2 ***)

(**

---

![agony](images/agony.png)

---

### Problem 2: Script <> App

' You should still see nothing. That's expected. You are running a script,
' and the script doesn't know how to access the config file. What's more,
' `ConfigurationManager doesn't access an `App.config` file at runtime;
' it accesses the program name `.dll.config` or `.exe.config`.
' We can solve this in one of two ways.

---

### Solution 1: FSharp.Configuration

' We'll now take a look at the FSharp.Configuration type provider.
' This is an excellent tool for accessing config file values and even 
' attempts to type check them for you.

---

*)

open FSharp.Configuration

type Settings1 = AppSettings<"App.config">

// Enter:
Settings1.Test2

(**

' What do you see when you try to enter `Settings1.Test2`?

---

    error FS0039: The field, constructor or member 'Test2' is not defined.

' The most obvious thing, and the one that works when using the type provider in a library
' or executable, fails in a script file. When using FSharp.Configuration in a script, you
' have to also set the executable file.

---

*)

Settings1.SelectExecutableFile "AppSettings.fsx"

Settings1.Test2

(**

' For a script, that is the script filename. Add the above lines and try to enter
' `Settings1.Test2` again. Now what do you see?

---

    error FS0039: The field, constructor or member 'Test2' is not defined.

' What's going on? If you think about it, you'll realize the problem. What's the
' name of the `config` file once you've compiled a library or executable project?
' It's Name.exe.config or Name.dll.config. FSharp.Configuration is anticipating the
' same thing here.

---

### Rename App.config to AppSettings.fsx.config

*)

type Settings2 = AppSettings<"AppSettings.fsx.config">
Settings2.SelectExecutableFile "AppSettings.fsx"

Settings2.Test2

(**

' Rename the App.config to AppSettings.fsx.config, the name the file would have
' if you were to compile an application and try it again.
' Note: I've renamed my Settings type here purely to help with generating slides.

---

    error FS0039: The field, constructor or member 'Test2' is not defined.

---

![agony](images/agony.png)

' Are you serious?! What is going on here? Why are you playing tricks on me?!
' This is the point where you begin to swear off scripting. Why must it be so
' hard?!

---

*)

let [<Literal>] AppSettingsConfig =
    __SOURCE_DIRECTORY__ + "/AppSettings.fsx.config"
type Settings = AppSettings<AppSettingsConfig>
let [<Literal>] AppSettingsExe =
    __SOURCE_DIRECTORY__ + "/AppSettings.fsx"
Settings.SelectExecutableFile AppSettingsExe

Settings.Test2

(**

' It turns out you need to specify the full path for this to work properly in scripts.
' You can use the `__SOURCE_DIRECTORY__` directive to git the path to the current script
' and then add the remaining file name part. This can be captured as a string literal,
' so you can use it in places like type provider parameters.
' If you run this now, you'll get the value from Test2.
' Try some of the other settings, and you'll see that the type provider is quite handy
' at returning values that are typed.

---

![ecstasy](images/ecstasy.png)

---

### Retrieve the value for AppKey

*)

(*** hide ***)
let [<Literal>] Config = __SOURCE_DIRECTORY__ + "/index.fsx.config"
type ScriptSettings = AppSettings<Config>
let [<Literal>] Exec = __SOURCE_DIRECTORY__ + "/index.fsx"
ScriptSettings.SelectExecutableFile Exec
let appKey1 = ScriptSettings.AppKey

(*** show ***)
Settings.AppKey

(**

---

*)

(*** include-value:appKey1 ***)

(**

' The poor type provider was tricked. It found a string with the same length and
' alphanumeric characteristics as a Guid, so it converted it.

---

![agony](images/agony.png)

---

### Solution 2: Override ConfigurationManager

' When all else fails, you can replace the System.Configuration.ConfigurationManager
' with a simple script.

---

### Configuration.fsx

*)

#r "System.Xml.dll"
#r "System.Xml.Linq.dll"
open System.Xml
open System.Xml.Linq

(**

' Create a new file called Configuration.fsx. At the top, add references to System.Xml.dll
' and System.Xml.Linq.dll, then open those namespaces.

---

### Configuration.fsx

*)

let [<Literal>] AppSettingsPath = __SOURCE_DIRECTORY__ + "/App.config"

(**

' Here we again use the absolute path string literal to specify the config file we want
' to reference. This makes the script less portable, but it's the only way to properly
' hijack the ConfigurationManager class, which uses static members.

---

### Configuration.fsx

*)

type ConfigurationManager() =
    static let config = XDocument.Load AppSettingsPath
    static let section (config:XDocument) name key value =
        query {
            for els in config.Descendants(XName.Get name) do
            for el in els.Descendants(XName.Get "add") do
            let k = el.Attribute(XName.Get key).Value 
            let v = el.Attribute(XName.Get value).Value 
            select (k,v)
        } |> dict
    static let appSettings =
        section config "appSettings" "key" "value"
    static let connectionStrings =
        section config "connectionStrings" "name" "connectionString"
    static member AppSettings = appSettings
    static member ConnectionStrings = connectionStrings

(**

' The guts of the ConfigurationManager class are not that complex. We have a static, let-bound
' function that loads and queries the provides config file and returns it as a dictionary, based
' on the section. I've only implemented the appSettings and connectionStrings portions here, but
' you can certainly add sections you want or need.

---

### Use Configuration.fsx

*)

#load "Configuration.fsx"
open Configuration

ConfigurationManager.AppSettings.["AppKey"]
(*** hide ***)
let appKey = ConfigurationManager.AppSettings.["AppKey"]
(*** include-value:appKey ***)

ConfigurationManager.ConnectionStrings.["Test1"]
(*** hide ***)
let test1 = ConfigurationManager.ConnectionStrings.["Test1"]

(*** include-value:test1 ***)

(**

' Go back to your AppSettings.fsx script and enter the following lines. Remembering that the
' script name is used as the namespace, we open Configuration after loading the file. So long
' as you load and open this Configuraiton after System.Configuration, this will hide the
' System.Configuration.ConfigurationManager, and you will be able to access whatever file you've
' specified in the Configuration.fsx script.

---

![ecstasy](images/ecstasy.png)

---

### Remember: Always Use `__SOURCE_DIRECTORY__`

' You now have two possible solutions you can leverage in your scripts. These strategies
' work for connection strings used with the various type providers, as well. It's always
' a good strategy to prefix a path to a config file with `__SOURCE_DIRECTORY__` to avoid
' issues with your scripts.

***

## Working with Data

' Let's start working on our ETL tool. First, we need to get some data.
' Create a new script named App.fsx.

---

### OpenCage GeoCoder

https://geocoder.opencagedata.com/

' We'll use OpenCage GeoCoder to retrieve a list of locations matching
' a specified query parameter.

---

*)

open FSharp.Data

let appKey = ConfigurationManager.AppSettings.["AppKey"]

let [<Literal>] ``geocoding.json`` =
    __SOURCE_DIRECTORY__ + "/geocoding.json"
type Location = JsonProvider<``geocoding.json``>

let extract city =
    let uri =
        "https://api.opencagedata.com/geocode/v1/json?q="
        + city
        + "&pretty=1&no_annotations=1&key="
        + appKey
    Location.Load(uri)

(**

' This is quite a bit of code, so let's break it down.
' First, we are using FSharp.Data.JsonProvider, so we
' need to open FSharp.Data
' I've taken the liberty to save a copy of a sample
' response. When using JsonProvider with scripts, it's
' often helpful to copy the response. In this case, the
' API call is rate limited, so using the actual url to
' request a response to generate types can cost you a
' number of requests, especially if you are reloading your
' interactive environment often.
' The response output is used to define a type called Location.
' The extract function accepts a city (or general location),
' populates the request URI with the input and the appKey,
' then loads the response into the Location type.

---

### Call `extract`

*)

let result = extract "Adelaide"

(**

' You should receive a result with one or more location results.
' It should match the format of the geocoding.json file.

---

## Leveraging Scripts for <acronym title="Extract-Transform-Load">ETL</acronym>

' We have some data, but what can you do with it? Let's look at how we might
' write a little extract-transform-load script.

---

### Defining Domain Types

*)

type LocationResult =
  { DisplayName : string
    Latitude : float
    Longitude : float
    City : string
    State : string
    Country : string
    CountryCode : string
    EnergyUse : Runtime.WorldBank.Indicator option }

(**

' While the type provider generates a type matching the source data,
' we don't want either the returned shape nor all the detail returned.
' Let's define our own LocationResult type into which we'll format
' our data set.

---

### Define `transformSimple`

*)

let transformSimple (source:Location.Root) : LocationResult list =
    [ for result in source.Results ->
        { DisplayName = result.Formatted
          Latitude = float result.Geometry.Lat
          Longitude = float result.Geometry.Lng
          City = result.Components.City
          State = result.Components.State
          Country = result.Components.Country
          CountryCode = result.Components.Iso31661Alpha2
          EnergyUse = None }
    ]

(**

' Here I've defined transformSimple, a version of our transform function
' that doesn't populate the EnergyUse property. More on that in a bit.
' This is not all that complex, and that's really the beauty of using
' F# for this type of work. Translations are simple functions mapping
' one type to another.

---

### World Bank Data

' However, we want additional data about energy use for the location.
' Let's use the World Bank type provider to look up energy use for the
' country location to demonstrate a) the ability to mix data sets from
' different sources easily and b) provide something more useful than
' simple location data for our ETL application.

---

### Define `transform`

*)

let extractEnergyUse (result:Location.Result)
        (wb:WorldBankData.ServiceTypes.WorldBankDataService) =
    wb.Countries
    |> Seq.tryFind (fun x ->
        result.Components.Country.StartsWith(x.Name))
    |> Option.map (fun x ->
        x.Indicators.``Energy use (kg of oil equivalent per capita)``)

let transform (source:Location.Root) : LocationResult list =
    let wb = WorldBankData.GetDataContext()
    [ for result in source.Results ->
        let energyUse = extractEnergyUse result wb
        { DisplayName = result.Formatted
          Latitude = float result.Geometry.Lat
          Longitude = float result.Geometry.Lng
          City = result.Components.City
          State = result.Components.State
          Country = result.Components.Country
          CountryCode = result.Components.Iso31661Alpha2
          EnergyUse = energyUse }
    ]

(**

' The World Bank type provider already understands how to translate
' HTTP requests from the World Bank and provides a number of indicators.
' Define extractEnergyUse to retrieve World Bank indicators for each
' result. We'll also parameterize the data context used to access
' the World Bank so we don't have to re-create it for each request.

---

### Persisting Data

' You may typically use SQL databases when manipulating data, but we'll
' use CSV for this exercise. Fortunately, FSharp.Data provides the CsvProvider
' with which we can read from and create CSV files.

---

### CsvProvider

*)

type LocationSink =
    CsvProvider<"Location (string),Latitude (float),Longitude (float),City (string),State (string),Country (string),CountryCode (string)">
type EnergyUseSink =
    CsvProvider<"CountryCode (string),EnergyUse (float)">

(**

' You can find the full headers in the LocationSinkHeaders.txt and
' EnergyUseSinkHeaders.txt files in the exercises folder.

---

### Define `load`

*)

let load (data:LocationResult list) =
    let locations, energyUse =
        [ for row in data ->
            let location =
                LocationSink.Row(
                    row.DisplayName, row.Latitude,
                    row.Longitude, row.City,
                    row.State, row.Country,
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

(**

' Once again, this is quite a lot of code in one snippet.
' We are defining a list comprehension that traverses each
' LocationResult and builds up pairs of LocationSink.Row and
' EnergyUseSink.Row list results. We then unzip them to separate
' the two lists.

---

### Define `load` (cont)

*)
    do
        use locationSink = new LocationSink(locations)
        locationSink.Save("locations.csv")
    do
        let distinct =
            List.concat energyUse
            |> List.distinctBy (fun x -> x.CountryCode)
        use energyUseSink = new EnergyUseSink(distinct)
        energyUseSink.Save("energyUse.csv")

(**

' Now that we have the results, we need to create the CSV files.
' We can load the row lists into their respective Sink types
' and call Save, passing a file name.
' For energyUse, since we may have multiple locations within the
' same country, let's make sure to capture only distinct results
' to avoid duplicates.
' You would follow a similar approach when working with a SQL database.

---

### Define `run`

*)

let run city =
    let data = (extract >> transform) city
    load data

(**

' Let's define a run function to make it easier to run everything together.
' Try it out. What sort of results are you getting? Look at the CSV results.
' Are they what you would expect?
' You can run just the `let data = (extract >> transform) "City"` to see the
' LocationResults directly.

---

### Iterating Design

' I'd like to point out at this point that this would be a good time to move
' our types to a library .fs file. However, we will leave them here for now.

---

![ecstasy](images/ecstasy.png)

***

## Testing

' Let's take an opportunity to test our code. There are multiple kinds of tests,
' and we'll start with some simple performance testing. This won't be anything
' spectacular; we'll simply leverage a directive available in F# Interactive.

---

### `#time`

    > #time;;

    --> Timing now on

    > run "Houston";;
    Real: 00:00:00.970, CPU: 00:00:00.093, GC gen0: 1, gen1: 0, gen2: 0
    val it : unit = ()

    > #time;;

    --> Timing now off

    >

' There are multiple kinds of tests,
' and we'll start with some simple performance testing. This won't be anything
' spectacular; we'll simply leverage a directive available in F# Interactive.

---

![ecstasy](images/ecstasy.png)

---

### Unit Testing with xUnit.net

' This won't work well for scripts since xunit uses a separate test runner
' that uses reflection to find tests using attributes.

---

![agony](images/agony.png)

---

### Unit Testing with NUnit

' See above.

---

![agony](images/agony.png)

---

### Unit Testing with Expecto

*)

#load "../.paket/load/net461/main.group.fsx"

open Expecto

(**

' At last, a testing library that will run in a script!
' First, create a new file, Tests.fsx, then open Expecto.

---

### Write a Test

*)

let addTest =
    testCase "An addition test" <| fun () ->
        let expected = 4
        Expect.equal expected (2+2) "2+2 = 4"

Tests.runTests defaultConfig addTest

(**

    [15:21:46 INF] EXPECTO? Running tests... <Expecto>
    [15:21:46 INF] EXPECTO! 1 tests run in 00:00:00.0297663 – 1 passed, 0 ignored, 0 failed, 0 errored. ᕙ໒( ˵ ಠ
    ╭͜ʖ╮ ಠೃ ˵ )७ᕗ <Expecto>
    val it : int = 0

' Let's start with a simple test. Well, that was easy. You define a testCase by
' giving it a name and a function to run. You verify the expected result using
' Expect.equal, which takes an expected value, the actual value, and a message
' if the test case fails.

---

### Write a Failing Test

*)

let failTest =
    testCase "Failing test" <| fun () ->
        Expect.equal 1 2 "1 <> 2"

Tests.runTests defaultConfig failTest

(**

    [15:29:39 INF] EXPECTO? Running tests... <Expecto>
    [15:29:39 ERR] Failing test failed in 00:00:00.0150000.
    1 <> 2. Actual value was 1 but had expected it to be 2.
      s:\scripting-workshop\slides\index.fsx(4,1): FSI_0005.failTest@4-21.Invoke(String x)
     <Expecto>
    [15:29:39 INF] EXPECTO! 1 tests run in 00:00:00.0270684 – 0 passed, 0 ignored, 1 failed, 0 errored. ( ರ Ĺ̯ ರ
    ೃ ) <Expecto>
    val it : int = 1

' Write a failing test. Here you can see the test case name and message in the output,
' making it quite easy to identify the culprit. 

---

*)

let multTest =
    testCase "A multiplication test" <| fun () ->
        let expected = 4
        Expect.equal expected (2*2) "2*2 = 4"

let intTests =
    testList "Integer math tests" [
        addTest
        multTest
    ]

Tests.runTests defaultConfig intTests

(**

    [15:27:37 INF] EXPECTO? Running tests... <Expecto>
    [15:27:37 INF] EXPECTO! 2 tests run in 00:00:00.0019015 – 2 passed, 0 ignored, 0 failed, 0 errored. ᕙ໒( ˵ ಠ
    ╭͜ʖ╮ ಠೃ ˵ )७ᕗ <Expecto>
    val it : int = 0

' In Expecto, tests are values, and you can group them and run them in their groups.

---

![ecstasy](images/ecstasy.png)

---

### FsCheck

*)

let config = { FsCheckConfig.defaultConfig with maxTest = 10000 }

let properties =
    testList "FsCheck samples" [
        testProperty "Addition is commutative" <| fun a b ->
            a + b = b + a

        testProperty "Reverse of reverse of a list is the original list" <|
            fun (xs:list<int>) -> List.rev (List.rev xs) = xs

        // you can also override the FsCheck config
        testPropertyWithConfig config "Product is distributive over addition" <|
            fun a b c -> a * (b + c) = a * b + a * c
    ]

Tests.runTests defaultConfig properties

(**

    [15:33:04 INF] EXPECTO? Running tests... <Expecto>
    [15:33:04 INF] EXPECTO! 3 tests run in 00:00:00.1008472 – 3 passed, 0 ignored, 0 failed, 0 errored. ᕙ໒( ˵ ಠ
    ╭͜ʖ╮ ಠೃ ˵ )७ᕗ <Expecto>

---

### Group testLists

*)

let allTests =
    testList "all tests" [
        intTests
        properties
    ]

Tests.runTests defaultConfig allTests

(**

    [15:34:09 INF] EXPECTO? Running tests... <Expecto>
    [15:34:09 INF] EXPECTO! 5 tests run in 00:00:00.1008795 – 5 passed, 0 ignored, 0 failed, 0 errored. ᕙ໒( ˵ ಠ
    ╭͜ʖ╮ ಠೃ ˵ )७ᕗ <Expecto

' Lastly, you can even group lists of tests and run them all together.

---

![ecstasy](images/ecstasy.png)

---

### Write some tests!

' Take a few minutes to write a few tests for the ETL functions we just wrote.
' Given we used type providers, it may be difficult to think through what tests
' to write. There's no correct answer. You may want to verify the CSV files were
' written or that the transform function returned 1 or more results.
' Don't forget to #load "App.fsx"!

***

## Visualizing Data Sets

### [XPlot](https://tahahachana.github.io/XPlot/)

*)

open XPlot.GoogleCharts

(**

' One of the benefits of scripts is being able to print out charts.
' XPlot provides several options, including GoogleCharts and Plotly.
' We'll use the GoogleCharts library for our sample app.

---

![ecstasy](images/ecstasy.png)

---

### Show Locations

*)

let showMap (data:LocationResult list) =
    data
    |> List.map (fun x -> x.Latitude, x.Longitude, x.DisplayName)
    |> Chart.Map
    |> Chart.WithOptions (Options(showTip = true))
    |> Chart.WithHeight 420
    |> Chart.Show

(**

' Call showMap.
' Since we are retrieving one or more location results, let's show them on a map.
' It's easier for the human mind to comprehend a visualization rather than a table
' of data, so rendering a chart can quickly help you zero in on the objective
' of the search.
' Using data you've retrieved from the transform function, call the showMap function.
' What do you see? 

---

#### Energy Use per Country

*)

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

(**

' Next, let's write a function that will take the energy use data we
' retrieved and render it in a line graph.

---

' Custom Printers

*)

fsi.AddPrinter(fun (chart:XPlot.GoogleCharts.GoogleChart) ->
   chart |> Chart.Show
   "Google Chart")

(**

' F# Interactive allows you to define custom printers. These are most
' common in validating data sets and UIs. We can now remove the calls
' to Chart.Show above.

---

### Add this to `run`

*)

let run city =
    let data = (extract >> transform) city
    load data
    showMap data
    showEnergyUse data

(**

' Let's add this to our run function so that we plot out the charts whenever
' the data set is retrieved. This will act as a visual cue that we have
' the correct data set. Feel free to add in any tests you created, as well.
' Try entering different locations into your interactive session and report
' on what you are seeing. "San Francisco" is quite interesting.

***

## Accessing Command Line Parameters

' We have not yet left scripts, yet we are also not done with our possibilities
' for practically leveraging scripts. So far we have been running whatever is
' defined in the script file. What if we want to run the script but parameterize
' it from the command line?

---

### PowerShell Wrappers

' My company currently has several F# scripts that are wrapped by PowerShell.
' The scripts work great, and we used PowerShell to provide better tab-completion.
' The PowerShell works fine, but I wondered whether F# could do it all.

---

### Argu

*)

open Argu

(**

' Argu is a terrific library that provides features for parsing command line arguments,
' defining command line arguments, defining subcommands, and printing CLI help.
' Arguu can also be used from a script file!

---

*)

type Arguments =
    | [<Mandatory>] City of string
    with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | City _ -> "Enter a location name"

(**

' Let's define the expected arguments. We expect a City that is of type string.
' The City parameter is mandatory. By default, this translates to `--city` on the CLI.
' The IArgParserTemplate is a required interface for working with Argu and provides
' the content displayed in the --help.

---

*)

let runCLI args =
    //let args = [|"Map.fsx";"--city";"Adelaide"|].[1..]
    let argParser = ArgumentParser.Create<Arguments>(errorHandler = ProcessExiter())
    let argResults = argParser.Parse(args)
    let city = argResults.GetResult <@ City @>
    run city

(**

' Next, let's use the Argu parser to extract values.
' I'm defining the errorHandler as a ProcessHandler to shut down
' any environments thtat break for some reason.
' Ultimately, we want to retrieve the City value and pass it along
' to run to execute our ETL and visualization code.
' That is all well and good, but how do you retrieve the command line
' arguments initially?

---

*)

let args = fsi.CommandLineArgs.[1..]
runCLI args

(**

' The fsi object has several useful properties, one of which is
' `fsi.CommandLinArgs`. This can be used to retrieve the CLI args
' for a script. However, make sure to trim off the first index,
' as that should include the name of the file read

---

### Run App.fsx with --city

    $ fsharpi exercises/App.fsx --city Houston

' Run your script now? What do you see? You should see two graphs opened in
' a web browser. You should also still see your generated CSV files.

---

### Paket.fsx

*)

#load "../exercises/Paket.fsx"
Paket.download()
Paket.restore()
Paket.generateLoadScripts "net461"

(**

' Take a look at exercises/Paket.fsx. Note that this exercises/ folder
' has no special requirements except to verify that the script is valid. 
' The above commands wrap the paket.exe command line executable.
' If we enter this toward the top of the script file, we will be able
' to effectively deploy and run everything necessary to run the script.
' > Take this opportunity to re-run the entire script.
' If you have correctly set your default paths, you should do so.
' You  don't remember ever haing to figur.

---

### Reconciliation

    fsharpi ./exercises/App.fsx --client Adelaide

' You can now specify the city from the command line. This allows us to
' pull off program data and turn it into something interesting for Tachyus.
'

***

## DEMO

### Using Scripts in a Project

---

### Compiled or Interactive

#### Why choose?

' Trying to run code in F# interactive that has been written
' as a compiled library or executable can sometimes be tricky.
' We'll look at techniques to make this less difficult.

***

## DEMO

### Debugging Scripts

***

## DEMO

### Build and Deploy Scripting with FAKE 

***

# Review

***

## Deployment Options

' As noted earlier, we are not able to go into the details of each option, 
' but I want you at least be aware of some of the options available.

---

### [Jupyter Notebooks](http://bayardrock.github.io/IfSharp/)

---

### [Azure Notebooks](https://blogs.msdn.microsoft.com/visualstudio/2016/12/05/azure-notebooks-now-support-f/)

---

### [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-fsharp)

---

### [AWS Lambda](https://github.com/FSharpBristol/FSharp-Template-for-Aws-Lambda)

---

### [FsReveal slides](http://fsprojects.github.io/FsReveal/)

' As an example, this slide deck is build using an F# script and FsReveal.

***

# Questions?

*)