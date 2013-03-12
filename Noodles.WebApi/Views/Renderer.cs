using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Noodles.WebApi.Models;
using RazorEngine;

namespace Noodles.WebApi.Views
{
    public class Renderer
    {
        public static string RenderForm(string typeName, object o, string prefix)
        {
            var type = GetType(typeName);

            var sb = new StringBuilder();

            if (type.IsValueType || type == typeof(System.Object) || type == typeof(System.String))
            {
                var inputGuid = Guid.NewGuid().ToString();
                sb.AppendLine("<label for=\"" + inputGuid + "\">" + prefix + "</label>");
                sb.AppendLine("<input id=\"" + inputGuid + "\" type=\"text\" value=\"" + (o ?? "") + "\" name=\"" + prefix + "\"/>");
            }
            else
            {
                if (o == null)
                {
                    o = Activator.CreateInstance(type);
                }
                foreach (var property in type.GetProperties())
                {
                    var value = property.GetValue(o);
                    var propertyType = value == null ? property.PropertyType
                        : value.GetType();
                    sb.AppendLine(RenderForm(propertyType.FullName, value, prefix + "." + property.Name));
                }
            }
            return sb.ToString();
        }

        private static Type GetType(string typeName)
        {
            Type type = null;
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = currentassembly.GetType(typeName, false, true);
                if (type != null) break;
            }
            return type;
        }

        public static void RenderView(StringBuilder sb, string viewName, object value)
        {
            var assembly = typeof(HtmlMediaTypeFormatter).Assembly;
            var resourceName = assembly.GetManifestResourceNames().Single(n => n.EndsWith("Views." + viewName + ".html"));
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                var template = reader.ReadToEnd();
                string result = Razor.Parse(template, value);
                sb.Append(result);
            }
        }
         

        public static string RenderView(string viewName, object value)
        {
            var sb = new StringBuilder();
            RenderView(sb, viewName, value);
            return sb.ToString();
        }

        public static void RenderBestView(object o)
        {
                
        }
    }
}
