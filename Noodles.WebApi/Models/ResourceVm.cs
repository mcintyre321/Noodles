using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Noodles.Models;
using Walkies;

namespace Noodles.WebApi.Models
{
    [KnownType(typeof(PropertyVm))]
    [KnownType(typeof(ActionVm))]
    [KnownType(typeof(LinkVm))]

    public class ResourceVm : NodeVm
    {
        //self
        //public string Name { get; set; }
        public PropertyVm[] Properties { get; set; }
        public ActionVm[] Actions { get; set; }
        public LinkVm[] Links { get; set; }
        public ResourceVm()
        {
            
        }
        public ResourceVm(Resource target)
        {
            //this.Name = target.Fragment;
            //this.DisplayName = target.DisplayName;
            this.Url = target.Url;
            //this.ValueType = target.ValueType.FullName;

            Properties = target.NodeProperties.Select(p => new PropertyVm(p)).ToArray();
            Actions = target.NodeMethods.Select(nm => new ActionVm(nm)).ToArray();
            var links = new List<LinkVm>();

            //if (target.Parent != null) links.Add(new LinkVm(target.Parent, "parent"));
            links.AddRange(target.Links.Select(c => new LinkVm(c, "child")).ToArray());
            this.Links = links.ToArray();
        }

        //public string DisplayName { get; set; }
    }
}