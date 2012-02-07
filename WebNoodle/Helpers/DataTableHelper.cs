using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Mvc.JQuery.Datatables;

namespace WebNoodle.Helpers
{
    public static class DataTableHelper
    {
        public static MvcHtmlString DataTableFor<TQueryable>(this HtmlHelper helper, Expression<Func<TQueryable>> getProperty, params string[] properties)
        {
            var target = GetObjectAndMember(getProperty);
            var vm = new DataTableVm("DataTable_" + target.Item2 + "_" + target.Item1.Id(), target.Item1.Path() + "?action=getDataTable&prop=" + target.Item2, properties);
            return helper.Partial("DataTable", vm);
        }
        public static MvcHtmlString DataTableForX<TIn, TResult>(this HtmlHelper helper, Expression<Func<IEnumerable<TIn>>> getProperty, Expression<Func<TIn, TResult>> transform)
        {
            var key = transform.ToString().GetHashCode().ToString(CultureInfo.InvariantCulture);
            var x = helper.ViewContext.HttpContext.Cache[key];
            if (x == null)
            {
                var compiled = transform.Compile();
                Func<object, IEnumerable<TResult>> objTransform = o => ((IEnumerable<TIn>)o).Select(compiled);
                helper.ViewContext.HttpContext.Cache.Add(key, objTransform, null, DateTime.MaxValue, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            var target = GetObjectAndMember(getProperty);
            var vm = new DataTableVm("DataTable_" + target.Item2 + "_" + target.Item1.Id(), target.Item1.Path() + "?action=getDataTable&prop=" + target.Item2 + "&transform=" + key, typeof(TResult).GetProperties().Select(p => p.Name));
            return helper.Partial("DataTable", vm);
        }


        private static Tuple<object, string, T> GetObjectAndMember<T>(Expression<Func<T>> getProperty)
        {
            dynamic exp = getProperty;
            var memberName = (string)exp.Body.Member.Name;
            var info = exp.Body.Expression.Member as object;
            object noodleObject = null;
            if (info is PropertyInfo)
            {
                noodleObject = exp.Body.Expression.Expression.Value.Model;
            }
            else if (info is FieldInfo)
            {
                var fi = (FieldInfo)info;
                noodleObject = fi.GetValue(exp.Body.Expression.Expression.Value);
            }
            else
            {
                throw new NotImplementedException(
                    "The static reflection code hasn't been able to figure out the object properly. Feel free to improve it!");
            }
            var queryable = (T)noodleObject.GetType().GetProperty(memberName).GetGetMethod().Invoke(noodleObject, null);
            return Tuple.Create(noodleObject, memberName, queryable);
        }
    }
}
