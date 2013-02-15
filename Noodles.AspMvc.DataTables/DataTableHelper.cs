using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Caching;
using System.Web.Mvc;
using Mvc.JQuery.Datatables;

namespace Noodles.AspMvc.DataTables
{
    public static class DataTableHelper
    {
        public static DataTableVm DataTableVmFor<T>(this HtmlHelper helper, Expression<Func<IEnumerable<T>>> getProperty)
        {
            return DataTableVmFor(helper, getProperty, x => x);
        }
        public static DataTableVm DataTableVmFor<TIn, TResult>(this HtmlHelper helper, Expression<Func<IEnumerable<TIn>>> getProperty, Expression<Func<TIn, TResult>> transform)
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
            var columns = typeof(TResult).GetProperties().Select(p => ColDef.Create(p.Name, p.Name, p.PropertyType)).ToArray();
            return new DataTableVm("DataTable_" + target.Item2 + "_" + target.Item1.Id(), target.Item1.Url + "?action=getDataTable&prop=" + target.Item2 + "&transform=" + key, columns);
        }


        private static Tuple<NodeProperty, string, string, T> GetObjectAndMember<T>(Expression<Func<T>> getProperty)
        {
            throw new NotImplementedException();
            //dynamic exp = getProperty;
            //var memberName = (string)exp.Body.Member.Name;
            //var info = exp.Body.Expression.Member as object;
            //object noodleObject = null;
            //var displayNameAtt = null as System.ComponentModel.DisplayNameAttribute;
            //if (info is PropertyInfo)
            //{
            //    noodleObject = exp.Body.Expression.Expression.Value.Model;
            //}
            //else if (info is FieldInfo)
            //{
            //    var fi = (FieldInfo)info;
            //    noodleObject = fi.GetValue(exp.Body.Expression.Expression.Value);
            //}
            //else
            //{
            //    throw new NotImplementedException(
            //        "The static reflection code hasn't been able to figure out the object properly. Feel free to improve it!");
            //}
            //var queryable = (T)noodleObject.GetType().GetProperty(memberName).GetGetMethod().Invoke(noodleObject, null);
            //return Tuple.Create(noodleObject, memberName, memberName, queryable);
        }
    }
}
