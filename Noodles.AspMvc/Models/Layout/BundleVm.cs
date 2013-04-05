using System.Collections.Generic;
using System.Text;

namespace Noodles.AspMvc.Models.Layout
{
    public class BundleVm
    {
        public IList<string> ScriptIncludes { get; private set; }
        public IList<string> StyleIncludes { get; set; }

        public BundleVm()
        {
            ScriptIncludes = new List<string>();
            StyleIncludes = new List<string>();
        }

        public string Render()
        {
            var sb = new StringBuilder();

            foreach (var script in ScriptIncludes)
            {
                sb.AppendLine("<script src = \"" + script + "\" type=\"text/javascript\" ></script >");
            }
            foreach (var script in StyleIncludes)
            {
                sb.AppendLine("<link href=\"" + script + "\" rel=\"stylesheet\" type=\"text/css\"/ >");
            }
            return sb.ToString();
        }
    }
}