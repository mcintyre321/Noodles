using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Noodles.Example.WebApi
{
    internal class NodeMethodHttpParameterDescriptor : HttpParameterDescriptor
    {
        private readonly NodeMethodParameter _parameter;

        public NodeMethodHttpParameterDescriptor(NodeMethodParameter parameter, HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            _parameter = parameter;
            this.Configuration = configuration;
            this.ActionDescriptor = actionDescriptor;
        }
        public override System.Collections.ObjectModel.Collection<T> GetCustomAttributes<T>()
        {
            var atts = base.GetCustomAttributes<T>().Concat(_parameter.CustomAttributes.OfType<T>()).ToList();
            var fromBody = new FromBodyAttribute();
            if (fromBody as T != null) atts.Add((T) (object) fromBody);
            var coll = new Collection<T>(new List<T>(atts));
            return coll;
        }
        #region Overrides of HttpParameterDescriptor

        public override string ParameterName
        {
            get { return _parameter.Name; }
        }

        public override Type ParameterType
        {
            get { return _parameter.ParameterType; }
        }

        #endregion
    }
}