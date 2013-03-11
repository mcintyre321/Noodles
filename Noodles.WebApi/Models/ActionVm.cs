using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class ActionVm
    {
        //self
        public string Name { get; set; }
        public string Url { get; set; }
 

        public LinkVm[] Links { get; set; }

        public ActionVm(NodeMethod target)
        {
            this.Name = target.Fragment;
            this.DisplayName = target.DisplayName;
            this.Url = target.Url;
            this.ParameterType = target.ParameterType.FullName;

            //Properties = target.NodeProperties.Select(p => new ResourceVm(p)).ToArray();
            //Actions = target.NodeMethods.Select(nm => new ResourceVm(nm)).ToArray();
            //var links = new List<LinkVm>();

            //if (target.Parent != null) links.Add(new LinkVm(target.Parent, "parent"));

            //links.AddRange(target.Children.Select(c => new LinkVm(c, "child")).ToArray());
            //this.Links = links.ToArray();
        }

        public string ParameterType { get; set; }

        public string DisplayName { get; set; }
    }
}