using System;
using Noodles.Models;

namespace Noodles.WebApi.Models
{
    public class ActionVm : NodeVm
    {
        //self
        public string Name { get; set; }

        public ActionVm(NodeMethod target)
        {
            this.Name = target.Name;
            this.DisplayName = target.DisplayName;
            this.Url = target.Url;
            this.RequestType = target.ParameterType.FullName;
            this.ResponseType = target.ResultType.FullName;
        }

        public string RequestType { get; set; }
        public string ResponseType { get; set; }
        public string DisplayName { get; set; }
    }
}