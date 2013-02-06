using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Noodles.AspMvc;
using Noodles.AspMvc.DataTables;

namespace Noodles.Example
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            RouteTable.Routes.MapHubs();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //This is the registration for noodles. note the wildcard 'path' parameter
            routes.MapRoute(
                           "Noodles", // Route name
                           "{*path}", // URL with parameters
                           new { controller = "Noodles", action = "Index", path = "/" } // Parameter defaults
                       );


            routes.MapRoute(
                            "Default", // Route name
                            "{controller}/{action}/{id}", // URL with parameters
                            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                        );



        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ActionResultExtension.Processors.Add(ActionResultProcessor.Rule);
        }
    }
}