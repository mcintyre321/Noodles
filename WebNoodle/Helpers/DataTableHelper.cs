using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.JQuery.Datatables;

namespace WebNoodle.Helpers
{
    public static class DataTableHelper
    {
        public static MvcHtmlString DataTableFor<TQueryable>(this HtmlHelper helper, Expression<Func<TQueryable>> getProperty, params string[] properties)
            where TQueryable : IQueryable
        {
            var target = GetObjectAndQueryable(getProperty);
            var vm = new DataTableVm("DataTable_" + target.Item2 + "_" + target.Item1.Id(), target.Item1.Path() + "?action=getDataTable&prop=" + target.Item2, properties);
            return helper.Partial("DataTable", vm);
        }

        private static Tuple<object, string, TQueryable> GetObjectAndQueryable<TQueryable>(Expression<Func<TQueryable>> getProperty) where TQueryable : IQueryable
        {
            dynamic exp = getProperty;
            var memberName = (string) exp.Body.Member.Name;
            var info = exp.Body.Expression.Member as object;
            object noodleObject = null;
            if (info is PropertyInfo)
            {
                noodleObject = exp.Body.Expression.Expression.Value.Model;
            }
            else if (info is FieldInfo)
            {
                var fi = (FieldInfo) info;
                noodleObject = fi.GetValue(exp.Body.Expression.Expression.Value);
            }
            else
            {
                throw new NotImplementedException(
                    "The static reflection code hasn't been able to figure out the object properly. Feel free to improve it!");
            }
            var queryable = (TQueryable) noodleObject.GetType().GetProperty(memberName).GetGetMethod().Invoke(noodleObject, null);
            return Tuple.Create(noodleObject, memberName, queryable);
        }
    }
}
