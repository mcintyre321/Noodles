using System.Threading.Tasks;
using System.Web.Mvc;
using Noodles.AspMvc;
using Noodles.Example.Domain;

namespace Noodles.Example.Controllers
{
    public class NoodlesController : Controller
    {
        static Application application = new Application();

        public async Task<ActionResult> Index(string path)
        {
            var root = SetUrlRootExtension.SetUrlRoot(application, this.ControllerContext);
            var actionResult = this.ControllerContext.GetNoodleResult(root);
            return await actionResult;
        }
    }
}
