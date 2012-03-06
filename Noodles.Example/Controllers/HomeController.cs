using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Noodles.Example.Models;
using Noodles;
namespace Noodles.Example.Controllers
{
    public class HomeController : Controller
    {
        static ToDoList home = new ToDoList();

        public ActionResult Index(string path)
        {
            var actionResult = this.ControllerContext.GetNoodleResult(home);
            return actionResult;
        }
    }
}
