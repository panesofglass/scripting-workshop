#r "System.Configuration.dll"
#load "Library.fs"
open Library
let x = MyClass(Name = "Some Name")
let result = x.Sum(2, 2)
(*** define-output: my-class ***)
printfn "%s: %i" x.Name result