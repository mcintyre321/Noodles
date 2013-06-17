using System.Threading.Tasks;
using System.Web.Mvc;
using Noodles.AspMvc;
using Noodles.AspMvc.Models.Layout;
using Noodles.Example.Domain;

namespace Noodles.Example.Controllers
{
    public class NoodlesController : Controller
    {
        Application application;

        public NoodlesController(Application application)
        {
            this.application = application;
        }

        public async Task<ActionResult> Index(string path)
        {
            this.HttpContext.LayoutVm().TopBar.Fixed = true;

            var actionResult = this.ControllerContext.GetNoodleResult(application);
            return await actionResult;
        }
    }
}
