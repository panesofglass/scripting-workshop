#r "System.Configuration.dll"
#load "Library.fs"
open Library
let x = MyClass(Name = "Some Name")
let result = x.Sum(2, 2)
printfn "%s: %i" x.Name result

printfn "%A" fsi.CommandLineArgs