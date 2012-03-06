using System.Web.Mvc;
using System.Web.WebPages;
using Noodles.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Noodles.App_Start.RegisterNoodles), "Start")]

namespace Noodles.App_Start {
    public static class RegisterNoodles {
        public static void Start() {
            GlobalFilters.Filters.Add(new GlobalFixUserExceptionsAttribute());
            GlobalFilters.Filters.Add(new ModelStateTempDataTransferAttribute());

        }
    }
}
