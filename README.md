#Noodles#

Noodles, built on .NET, is a what you might call an OIM (Object-Interface mapper). By simply marking your classes up to
indicate relationships between domain objects, and to expose methods and properties (based on dynamic rules), you can build an application or API.

Currently there are two implementations:

 - Noodles.AspMvc: build full featured maintainable HTML web applications. Progressively enhance your UI conventions to increase UI quality, and provide overrides for custom features.
 - Noodles.WebApi: build rich Hypermedia APIs with HATEOAS compliance and ContentType versioning.

####Live Example MVC application: http://noodles.example.web.5.9.55.208.xip.io/####

####Live Example WebApi application: http://noodles.example.web.5.9.55.208.xip.io/api####

## Key Benefits ##

 - Reusable UI conventions across your site
 - Standardised API interaction
 - Very fast development
 - Dynamic, cross cutting security features
 - Opt in - you can use it for your whole app, or just alongside a standard app

## Features ##

 * MIT Licence
 * Routable within existing ASP MVC and WebAPi applications. You can use it just for your admin area if you want.
 * Customisable UI conventions
 * Call methods via Post-Redirect-Get, with automatic client-side validation
 * Control access to properties and actions dynamically using [Harden][2]
 * Built on top of standard ASP MVC/WebApi
 * Database agnostic. You have control over persistence, so you can use SQL, Raven/Mongo/Couch or something clever you have rolled yourself.
 * Realtime UI sync using SignalR

## Getting started##

**a.** Create an empty MVC4 or WebApi application

**b.** Install the following Nuget packages (the dependencies aren't set up yet, sorry!)

For MVC

    Install-Package Noodles
    Install-Package Noodles.AspMvc
    Install-Package EmbeddedResourceVirtualPathProvider
    
For WebApi

    Install-Package Noodles
    Install-Package Noodles.WebApi (for WebApi)

**c.** Create your [domain model][3], marked up with [ShowAttribute] to expose properties and methods, and [Parent] and [Child] attributes to indicate relationships between objects. Use [Harden][2] methods and attributes to secure your system.

**d.** add a wildcard route to your application ([AspMvc][4] / [WebApi][5]), and for MVC, [a controller][6], passing the root of your object graph.

**e** at this point you should be able to start your app, and have a functional application. Amazing!

alternatively, check out the [Example][8] [projects][9]!

##Future work##

 - Mobile UI
 - Realtime object sync using SignalR
 - PJax
 - Inline property editing

##Finally##

Please don't be a vacuum - emails, tweets, follows and questions actively sought after!

thanks - Harry McIntyre @mcintyre321

  [1]: https://github.com/mcintyre321/Harden
  [2]: https://github.com/mcintyre321/Harden
  [3]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Domain/Application.cs
  [4]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Web/Global.asax.cs
  [5]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.WebApi/Program.cs
  [6]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Web/Controllers/NoodlesController.cs
  [7]: https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Web/Views/Shared/_Layout.cshtml
  [8]: https://github.com/mcintyre321/Noodles/tree/master/Noodles.Example.Web
  [9]: https://github.com/mcintyre321/Noodles/tree/master/Noodles.Example.WebApi
  
  [![githalytics.com alpha](https://cruel-carlota.pagodabox.com/caba06ced732280266dfe94e5b4de886 "githalytics.com")](http://githalytics.com/mcintyre321/Noodles)
