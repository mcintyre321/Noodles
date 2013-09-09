using System.Threading.Tasks;
using System.Web.Mvc;
using Noodles.AspMvc;
using Noodles.AspMvc.Helpers;
using Noodles.AspMvc.Models.Layout;
using Noodles.Example.ActionFilters;
using Noodles.Example.Domain;

namespace Noodles.Example.Controllers
{
    [Turbolinks]
    public class NoodlesController : Controller
    {
        public async Task<ActionResult> Index(string path)
        {
            this.HttpContext.LayoutVm().RegisterScripts();

            var actionResult = this.ControllerContext.GetNoodleResult(CurrentApplication);
            return await actionResult;
        }

        //this is a method for creating a dummy application object for the current user to play with
        private Application CurrentApplication
        {
            get
            {
                var app = Session["Application"] as Application;
                if (app != null)
                {
                    return app;
                }
                app = new Application();
                DummyData.SeedApplication(app);
                Session["Application"] = app;
                return app;
            }
        }

    }
}
