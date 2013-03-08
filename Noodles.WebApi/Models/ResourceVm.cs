using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Noodles.Models;
using Walkies;

namespace Noodles.WebApi.Models
{
    public class ResourceVm
    {
        //self
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        public ResourceVm[] Properties { get; set; }
        public ResourceVm[] Actions { get; set; }


        public LinkVm[] Links { get; set; }

        public ResourceVm(INode target)
        {
            this.Name = target.DisplayName;
            this.Url = target.Url;
            this.Type = target.TypeName;

            Properties = target.NodeProperties.Select(p => new ResourceVm(p)).ToArray();
            Actions = target.NodeMethods.Select(nm => new ResourceVm(nm)).ToArray();
            var links = new List<LinkVm>();

            if (target.Parent != null) links.Add(new LinkVm(target.Parent, "parent"));

            links.AddRange(target.Children.Select(c => new LinkVm(c, "child")).ToArray());
            this.Links = links.ToArray();
        }



    }
}