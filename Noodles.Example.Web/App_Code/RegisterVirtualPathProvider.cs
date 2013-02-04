namespace Noodles.Example
{
    public class RegisterVirtualPathProvider
    {
        public static void AppInitialize()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceVirtualPathProvider.Vpp()
            {
				{typeof(AspMvc.MvcApplication).Assembly, @"..\Noodles.Web"} ,
                {typeof(Mvc.JQuery.Datatables.DataTablesData).Assembly} ,

                typeof(FormFactory.Logger).Assembly,
            });
        }
    }
}