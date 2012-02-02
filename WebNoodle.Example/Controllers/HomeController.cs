using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoodle.Example.Models;

namespace WebNoodle.Example.Controllers
{
    public class HomeController : Controller
    {
        static ToDoList home = new ToDoList();

        public ActionResult Index(string path)
        {
            var noodle = new NoodleResultBuilder();
            var actionResult = noodle.Execute(this.ControllerContext, home.YieldChildren(path).Last());
            return actionResult;
        }
    }
}
