using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class PropertyVm
    {
        //self
        public string Name { get; set; }
        public Uri Url { get; set; }
        public object Value { get; set; }
        public string ValueType { get; set; }
        public ActionVm[] Actions { get; set; }


        public PropertyVm(NodeProperty target)
        {
            this.Name = target.Fragment;
            this.Url = target.Url;
            this.Value = target.Value;
            this.ValueType = target.ValueType.FullName;

            Actions = target.NodeMethods.Select(nm => new ActionVm(nm)).ToArray();
        }
    }
}