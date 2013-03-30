using System;
using System.Web.Http;
using System.Web.Http.Routing;
using Newtonsoft.Json;
using Noodles.Example.Domain;
using Noodles.WebApi;

namespace Noodles.Example.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var settings = configuration.Formatters.JsonFormatter.SerializerSettings;
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
            //configuration.Formatters.Add(new HtmlMediaTypeFormatter());
            configuration.Routes.Add("Api", configuration.Routes.CreateRoute("api/{*path}",
                   new HttpRouteValueDictionary("route"),
                   constraints: null,
                   dataTokens: null,
                   handler: new NoodlesHttpMessageHandler((r) => Munq.MVC3.MunqDependencyResolver.Container.Resolve<Application>())
                   ));
        }
    }

    
}