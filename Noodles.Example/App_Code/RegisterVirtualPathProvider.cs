using System.Reflection;
using System.Linq;
namespace WebNoodle.Example.App_Code
{
    public class RegisterVirtualPathProvider
    {
        public static void AppInitialize()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp()
            {
				{typeof(Noodles.Web.MvcApplication).Assembly, @"..\Noodles.Web"} ,
                {typeof(Mvc.JQuery.Datatables.DataTablesData).Assembly} ,

                typeof(FormFactory.Logger).Assembly,
            });
        }
    }
}