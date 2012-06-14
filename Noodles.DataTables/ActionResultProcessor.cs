﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Mvc.JQuery.Datatables;

namespace Noodles.DataTables
{
    public class ActionResultProcessor
    {
        public static ActionResult Rule(ControllerContext cc, object node)
        {
            var request = cc.HttpContext.Request;
            if (request.HttpMethod.ToLowerInvariant() == "post" && request.QueryString["action"] == "getDataTable")
            {
                using (Profiler.Step("Returning DataTable"))
                {
                    var propertyName = request.QueryString["prop"];
                    var queryable = node.GetType().GetProperty(propertyName).GetGetMethod().Invoke(node, null);
                    var transformKey = request.QueryString["transform"];
                    if (transformKey != null)
                    {
                        dynamic transform = cc.HttpContext.Cache[transformKey];
                        queryable = transform.Invoke(queryable);
                    }
                    var dtr = Mvc.JQuery.Datatables.DataTablesResult.Create(queryable, ActionResultExtension.BindObject<DataTablesParam>(cc, "dataTableParam"));
                    return dtr;
                }
            }
            return null;
        }
    }
}
