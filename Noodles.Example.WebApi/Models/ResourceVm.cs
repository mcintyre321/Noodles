using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles.Example.WebApi.Models
{
    public class ResourceVm
    {
        public string Name { get; set; }
        public PropertyVm[] Properties { get; set; }
        public ResourceVm[] Actions { get; set; }
        public string Url { get; set; }
        public LinkVm Parent { get; set; }
        public LinkVm[] Children { get; set; }

        public ResourceVm(object target)
        {
            this.Url = target.Url();
            Properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute<ShowAttribute>() != null)
                .Select(p => new PropertyVm(target, p)).ToArray();
            Actions = target.NodeMethods().Select(nm => new ResourceVm(nm)).ToArray();
            this.Parent = target.Parent() == null ? null : new LinkVm(target.Parent(), "parent", "Parent");
            this.Children = target.KnownChildren().Select(c => new LinkVm(c, "child", c.GetFragment())).ToArray();

        }

        public ResourceVm(NodeMethod method)
        {
            Properties = method.Parameters.Select(p => new PropertyVm(p)).ToArray();
            this.Url = method.Url();
            this.Parent = new LinkVm(method.Parent, "parent", "Parent");
            this.Children = new LinkVm[]{};
            Actions = new ResourceVm[]{};
            this.Name = method.Name;
        }

    }
}