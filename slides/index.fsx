(**
- title : The Agony and the Ecstasy of F# Scripting
- description : Discover the wonderful world of F# scripting, including its agonies and ecstasies. We'll work through building a simple application starting with a script and moving to the relevant library and test projects and discuss strategies for managing app settings and connection strings, as well as how to access command line arguments.
- author : Ryan Riley
- theme : night
- transition : default

***

## The Agony and the Ecstasy of F# Scripting

***

## Use Cases

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

1. Documentation
2. Slide decks
3. Literate programming

---

### Build and Deploy

---

### Configuration

---

### Runtime!

1. Azure Functions
2. AWS Lambda
3. etc.

***

## Common Problems

---

### References

---

### Current Directory

---

### Accessing App.config

---

### Compiled or Interactive

#### Why choose?

' Trying to run code in F# interactive that has been written
' as a compiled library or executable can sometimes be tricky.
' We'll look at techniques to make this less difficult.

***

*)