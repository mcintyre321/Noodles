#Noodles#



Noodles is a .NET framework for building model driven web/API/mobile* applications on top of ASP MVC or WebApi.

Simply mark your classes up to indicate relationships between domain objects, and to expose methods and properties. Add a route to the Mvc or WebApi handler, and you have generated your web application!

* mobile coming soon


## Key Benefits ##

 - Standardised UI convention across your site
 - Standardised API interaction
 - Very fast development
 - Dynamic, cross cutting security features
 - Opt in - you can use it for your whole app, or just alongside a standard app

##Features##

 * Routable within existing ASP MVC and WebAPi applications. You can use it just for your admin area if you want.
 * Customisable UI conventions
 * Call methods via Post-Redirect-Get, with automatic client-side validation
 * Control access to properties and actions dynamically using [Harden][2]
 * Built on top of standard ASP MVC/WebApi
 * Database agnostic. You have control over persistence, so you can use SQL, Raven/Mongo/Couch or something clever you have rolled yourself.

## Getting started##

**a.** Create, or have a .NET 4.5 MVC4 or WebApi application.

**b.** Install the following Nuget packages (the dependencies aren't set up yet, sorry!)

For MVC

    Install-Package Noodles
    Install-Package Noodles.AspMvc
    Install-Package EmbeddedResourceVirtualPathProvider
    
For WebApi

    Install-Package Noodles
    Install-Package Noodles.WebApi (for WebApi)

**c.** Create your [domain model][3], marked up with [ShowAttribute] to expose properties and methods, and [Parent] and [Child] attributes to indicate relationships between objects. Use [Harden][2] methods and attributes to secure your system.

**d.** add a wildcard route to your application ([AspMvc][4][WebApi][5]), , giving it the 'Root' object of your object graph.

##Future work##

 - Mobile UI
 - Realtime object sync using SignalR
 - PJax
 - Inline property editing


  [1]: https://github.com/mcintyre321/Harden
  [2]: https://github.com/mcintyre321/Harden
  [3]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Domain/Application.cs
  [4]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Web/Global.asax.cs
  [5]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.WebApi/Program.cs
