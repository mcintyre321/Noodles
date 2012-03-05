using System.Web.Mvc;
using System.Web.WebPages;
using WebNoodle.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(WebNoodle.App_Start.RegisterWebNoodle), "Start")]

namespace WebNoodle.App_Start {
    public static class RegisterWebNoodle {
        public static void Start() {
            GlobalFilters.Filters.Add(new GlobalFixUserExceptionsAttribute());
            GlobalFilters.Filters.Add(new ModelStateTempDataTransferAttribute());

        }
    }
}
