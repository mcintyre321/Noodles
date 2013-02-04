using System;
using System.Reflection;
using System.Text;
using System.Web.Http.ModelBinding;
using System.Web.Http.Routing;
using System.Web.Http.SelfHost;
using Noodles.Example.Domain;
using Noodles.WebApi;

namespace Noodles.Example.WebApi
{
    public class Program
    {

        public static void Main(string[] args)
        {


            HttpSelfHostServer server = null;
            try
            {
                var config = new HttpSelfHostConfiguration("http://localhost:3002/")
                {
                };
                config.Formatters.Add(new HtmlMediaTypeFormatter());
                var todoList = new Application();
                config.Routes.Add("Noodles", config.Routes.CreateRoute("{*path}",
                    new HttpRouteValueDictionary("route"),
                    constraints: null,
                    dataTokens: null,
                    handler: new NoodlesHttpMessageHandler((r) => todoList)
                    ));
                server = new HttpSelfHostServer(config);

                server.OpenAsync().Wait();

                Console.WriteLine("Hit ENTER to exit");
                Console.ReadLine();
            }
            finally
            {
                if (server != null)
                {
                    server.CloseAsync().Wait();
                }
            }


        }
    }
}
