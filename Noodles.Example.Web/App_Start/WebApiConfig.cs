using System.Web.Http;
using System.Web.Http.Routing;
using Noodles.Example.Domain;
using Noodles.WebApi;

namespace Noodles.Example.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
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