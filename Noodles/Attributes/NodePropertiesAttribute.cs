using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles
{
    public class NodePropertiesAttribute : Attribute
    {
        public static IEnumerable<NodeProperty> GetDynamicNodeProperties(object o, INode resource)
        {
            if (o == null) yield break;
            var dynamicPropertiesMethods = o.GetType().AllMethodInfos().Where(m => m.Attributes().OfType<NodePropertiesAttribute>().Any());
            foreach (var dynamicPropertiesMethod in dynamicPropertiesMethods)
            {
                var result = (IEnumerable) dynamicPropertiesMethod.Invoke(o, new object[] {resource});
                foreach (NodeProperty nodeProperty in result)
                {
                    yield return nodeProperty;
                }
            }
        }
    }
}