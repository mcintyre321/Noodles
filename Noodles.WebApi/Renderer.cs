using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Noodles.WebApi
{
    public class Renderer
    {
        public static string RenderForm(string typeName, object o, string prefix)
        {
            Type type = null;
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = currentassembly.GetType(typeName, false, true);
                if (type != null) break;
            }

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
                    var propertyType = value == null ? property.PropertyType : value.GetType();
                    sb.AppendLine(RenderForm(propertyType.FullName, value, prefix + "." + property.Name));
                }
            }
            return sb.ToString();
        }
    }
}
