using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Munq;
using Noodles.AspMvc;
using Noodles.AspMvc.DataTables;
using Noodles.Example.App_Start;

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
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            WebApiConfig.Register(GlobalConfiguration.Configuration);

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


            Domain.AuthService.RequestHasAuthToken = () => HttpContext.Current == null || HttpContext.Current.Request.IsAuthenticated;
            Domain.AuthService.SetAuthToken = (key) =>
                {
                    if (HttpContext.Current != null)
                    {
                        FormsAuthentication.SetAuthCookie(key, true);
                    }
                };
            Domain.AuthService.ClearAuthToken = () =>
                {
                    if (HttpContext.Current != null)
                    {
                        FormsAuthentication.SignOut();
                    }
                };
            Domain.AuthService.GetAuthTokenKey = () => HttpContext.Current == null
                                                           ? "asdf"
                                                           : (HttpContext.Current.Request.IsAuthenticated ? null : HttpContext.Current.User.Identity.Name);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            Munq.MVC3.MunqDependencyResolver.Container.Register<Domain.Application, Domain.Application>().AsContainerSingleton();
            //ActionResultExtension.Processors.Add(ActionResultProcessor.Rule);
        }
    }
}