using System.Web.Mvc;
using Noodles.AspMvc.App_Start;
using Noodles.AspMvc.Infrastructure;

[assembly: WebActivator.PreApplicationStartMethod(typeof(RegisterNoodles), "Start")]

namespace Noodles.AspMvc.App_Start {
    public static class RegisterNoodles {
        public static void Start() {
            GlobalFilters.Filters.Add(new GlobalFixUserExceptionsAttribute());
            GlobalFilters.Filters.Add(new ModelStateTempDataTransferAttribute());

        }
    }
}
