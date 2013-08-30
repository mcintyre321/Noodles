using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Noodles.Models;


namespace Noodles.WebApi.Models
{
    [KnownType(typeof(PropertyVm))]
    [KnownType(typeof(ActionVm))]

    public class ResourceVm : NodeVm
    {
        //self
        //public string Name { get; set; }
        public PropertyVm[] Properties { get; set; }
        public ActionVm[] Actions { get; set; }
        public ResourceVm()
        {
            
        }
        public ResourceVm(Resource target)
        {
            //this.Name = target.Fragment;
            //this.DisplayName = target.DisplayName;
            this.Url = target.Url;
            //this.ValueType = target.ValueType.FullName;

            //Properties = target.NodeProperties.Select(p => new PropertyVm(p)).ToArray();
            //Actions = target.NodeMethods.Select(nm => new ActionVm(nm)).ToArray();

            //if (target.Parent != null) links.Add(new LinkVm(target.Parent, "parent"));
        }

        //public string DisplayName { get; set; }
    }
}