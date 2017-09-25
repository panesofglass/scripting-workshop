(**
- title : The Agony and the Ecstasy of F# Scripting
- description : Discover the wonderful world of F# scripting, including its agonies and ecstasies. We'll work through building a simple application starting with a script and moving to the relevant library and test projects and discuss strategies for managing app settings and connection strings, as well as how to access command line arguments.
- author : Ryan Riley
- theme : night
- transition : default

***

## The Agony and the Ecstasy of F# Scripting

[![The Agony and the Ecstasy](images/the-agony-and-the-ecstasy.jpg)](https://emerdelac.files.wordpress.com/2012/05/the-agony-and-the-ecstasy.jpg)

***

## Use Cases

![ecstasy](images/ecstasy.png)

---

### Design

' Design applies to both domain modeling and data modeling.
' For domain modeling, you may want to use scripts to understand
' the ramifications of certain design decisions. For example,
' is a record the right type, or might you need a discriminated
' union to better cover multiple cases, especially if you may need
' more in the future. How might units of measure help or hinder?
' With respect to data modeling, it can be helpful to throw the
' data in a plot. Interactive sessions work well for this, and you
' can check calculations more easily with visualizations. (Reference?)

---

### Testing

---

### Communicating

1. [Documentation](http://fsprojects.github.io/ProjectScaffold/)
2. [Slide decks](http://fsprojects.github.io/FsReveal/)
3. [Literate programming](http://fsprojects.github.io/FSharp.Formatting/literate.html)

---

### Build and Deploy

---

### Scheduled Tasks
 
---

### Configuration

* FAKE

---

### Runtime!

1. Azure Functions
2. AWS Lambda
3. etc.

***

## Common Problems

![agony](images/agony.png)

---

### References

---

### Current Directory

' When running F# interactive, the current directory is often
' not what you think.
' Try running `System.Environment.CurrentDirectory` and see
' what FSI returns.

---

### Accessing App.config

---

### Compiled or Interactive

#### Why choose?

' Trying to run code in F# interactive that has been written
' as a compiled library or executable can sometimes be tricky.
' We'll look at techniques to make this less difficult.

***

## Deployment options

---

### [iFSharp Notebooks](http://bayardrock.github.io/IfSharp/)

---

### [Azure Notebooks](https://blogs.msdn.microsoft.com/visualstudio/2016/12/05/azure-notebooks-now-support-f/)

---

### [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-reference-fsharp)

---

### [AWS Lambda](https://github.com/FSharpBristol/FSharp-Template-for-Aws-Lambda)

---

### FsReveal slides

***

*)