(**
- title : The Agony and the Ecstasy of F# Scripting
- description : Discover the wonderful world of F# scripting, including its agonies and ecstasies. We'll work through building a simple application starting with a script and moving to the relevant library and test projects and discuss strategies for managing app settings and connection strings, as well as how to access command line arguments.
- author : Ryan Riley
- theme : night
- transition : default

***
*)

(*** hide ***)
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
* Loading and referencing files
* Referencing NuGet packages
* Accessing values from .config files
* Retrieving data
* Design iteration
* Testing
* Visualizing data sets
* Accessing command line parameters
* Leveraging scripts for <acronym title="Extract-Transform-Load">ETL</acronym>
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

    ./.paket/paket.exe generate-load-scripts --group main --type fsx --framework net462

' Run the paket command to generate fsx load scripts for group main and
' framework net462. This will generate the references necessary for our
' use and no more. Note that you can specify different groups and types,
' the other type being csx for C# Interactive.
' NOTE: if you are on Mac or Linux, you'll need to prefix the above with `mono`.

---

### Load Dependencies

*)

#load "../.paket/load/net462/main.group.fsx"

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

let appSettings = ConfigurationManager.AppSettings
(*** define-value: app-settings-test2 ***)
appSettings.["test2"]

(*** include-value: app-settings-test2 ***)

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

(*** define-output: fsi-current-directory ***)
printfn "%s" System.Environment.CurrentDirectory

(*** include-output: fsi-current-directory ***)

(**
 
' The first problem is that the script is not necessarily running it its own folder.
' The current directory returned is the directory in which F# Interactive is running.
' Depending on where and how you run your script, you may not be able to access
' the config file you want.
 
---

### Change the Current Directory

*)

System.Environment.CurrentDirectory <- __SOURCE_DIRECTORY__
(*** define-output: set-current-directory ***)
printfn "%s" System.Environment.CurrentDirectory

(*** include-output: set-current-directory ***)

(**

' We can set the current directory using this line of code.
' You'll typically want to work from your source directory.
' Try adding this line just after opening `System.Configuration`,
' restart your environment, and try again.

---

(*** include-value: app-settings-test2 ***)

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

### FSharp.Configuration Type Provider

' We'll now take a look at the FSharp.Configuration type provider.
' This is an excellent tool for accessing config file values and even 
' attempts to type check them for you.

***

## Retrieving Data

***

## Design Iteration

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

## Testing

***

## Visualizing Data Sets

***

## Accessing Command Line Parameters

***

## Leveraging Scripts for <acronym title="Extract-Transform-Load">ETL</acronym>

***

## Using Scripts in a Project

***

## Debugging Scripts

***

## Build and Deploy Scripting with FAKE 

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

***

![agony](images/agony.png)
![ecstasy](images/ecstasy.png)

---

### Communicating

1. [Documentation](http://fsprojects.github.io/ProjectScaffold/)
3. [Literate programming](http://fsprojects.github.io/FSharp.Formatting/literate.html)

---

### Scheduled Tasks
 
---

### Current Directory

' When running F# interactive, the current directory is often
' not what you think.
' Try running `System.Environment.CurrentDirectory` and see
' what FSI returns.

---

### Compiled or Interactive

#### Why choose?

' Trying to run code in F# interactive that has been written
' as a compiled library or executable can sometimes be tricky.
' We'll look at techniques to make this less difficult.

*)