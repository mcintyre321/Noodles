using System.Collections.Generic;
using System.Linq;

namespace Noodles
{

    public static class NodeExtensions
    {
        public static IEnumerable<NodeMethod> NodeMethods(this object o)
        {
            return NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(o);
        }
        public static NodeMethod NodeMethod(this object o, string methodName)
        {
            return o.NodeMethods().SingleOrDefault(m => m.Name.ToLowerInvariant() == methodName.ToLowerInvariant());
        }
        public static IEnumerable<NodeProperty> NodeProperties(this object o)
        {
            return NodePropertiesReflectionLogic.YieldFindNodePropertiesUsingReflection(o);
        }
        public static NodeProperty NodeProperty(this object o, string propertyName)
        {
            return o.NodeProperties().SingleOrDefault(m => m.Name.ToLowerInvariant() == propertyName.ToLowerInvariant());
        }

    }
}