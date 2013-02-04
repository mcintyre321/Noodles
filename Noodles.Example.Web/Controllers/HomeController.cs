using System.Web.Mvc;
using Noodles.AspMvc;
using Noodles.Example.Domain;

namespace Noodles.Example.Controllers
{
    public class HomeController : Controller
    {
        static Application application = new Application();

        public ActionResult Index(string path)
        {
            var root = SetUrlRootExtension.SetUrlRoot(application, this.ControllerContext);
            var actionResult = this.ControllerContext.GetNoodleResult(root);
            return actionResult;
        }
    }
}
