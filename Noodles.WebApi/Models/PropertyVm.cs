using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class PropertyVm
    {
        //self
        public string Name { get; set; }
        public string Url { get; set; }
        public object Value { get; set; }
        public string ValueType { get; set; }
        public ActionVm[] Actions { get; set; }


        public PropertyVm(NodeProperty target)
        {
            this.Name = target.Fragment;
            this.DisplayName = target.DisplayName;
            this.Url = target.Url;
            this.Value = target;
            this.ValueType = target.ValueType.FullName;

            Actions = target.NodeMethods.Select(nm => new ActionVm(nm)).ToArray();
        }

        public string DisplayName { get; set; }
    }
}