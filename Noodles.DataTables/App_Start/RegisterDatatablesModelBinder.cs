using System.Web.Mvc;


[assembly: WebActivator.PreApplicationStartMethod(typeof(Noodles.DataTables.App_Start.RegisterDatatablesModelBinder), "Start")]

namespace Noodles.DataTables.App_Start {
    public static class RegisterDatatablesModelBinder {
        public static void Start() {
            if (!ModelBinders.Binders.ContainsKey(typeof(Mvc.JQuery.Datatables.DataTablesParam)))
                ModelBinders.Binders.Add(typeof(Mvc.JQuery.Datatables.DataTablesParam), new Mvc.JQuery.Datatables.DataTablesModelBinder());
        }
    }
}
