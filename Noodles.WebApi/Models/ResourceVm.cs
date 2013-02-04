using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Walkies;

namespace Noodles.WebApi.Models
{
    public class ResourceVm
    {
        //self
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        public PropertyVm[] Properties { get; set; }
        public ResourceVm[] Actions { get; set; }


        public LinkVm[] Links { get; set; }

        public ResourceVm(object target)
        {
            this.Name = target.GetName();
            
            this.Url = target.Url();
            this.Type = target.GetType().Name;

            Properties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute<ShowAttribute>() != null)
                .Select(p => new PropertyVm(target, p)).ToArray();
            Actions = target.NodeMethods().Select(nm => new ResourceVm(nm)).ToArray();
            var links = new List<LinkVm>();

            if (target.Parent() != null) links.Add(new LinkVm(target.Parent(), "parent", "Parent"));

            links.AddRange(target.KnownChildren().Select(c => new LinkVm(c, "child", c.GetFragment())).ToArray());
            this.Links = links.ToArray();
        }

        public ResourceVm(NodeMethod method)
        {
            this.Url = method.Url();
            this.Name = method.Name;
            this.Type = method.GetType().Name;

            Properties = method.Parameters.Select(p => new PropertyVm(p)).ToArray();
            this.Links = new[] { new LinkVm(method.Parent.Parent, "parent", "Parent") };
            Actions = new ResourceVm[]{};
        }

    }
}