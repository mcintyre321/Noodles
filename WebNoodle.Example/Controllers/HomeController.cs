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
        static Home home = new Home();

        public ActionResult Index(string path)
        {
            var noodle = new NoodleResultBuilder();
            return noodle.Execute(this.ControllerContext, home.YieldChildren(path).Last());
        }
    }
}
