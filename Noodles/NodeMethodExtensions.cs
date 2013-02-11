using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public static class NodeMethodExtensions
    {
        public static IEnumerable<NodeMethod> NodeMethods(this object o)
        {
            return NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(o)
                .Where(nm => !nm.Name.StartsWith("set_"));
        }

        public static NodeMethod NodeMethod(this object o, string methodName)
        {
            return o.NodeMethods().SingleOrDefault(m => m.Name.ToLowerInvariant() == methodName.ToLowerInvariant());
        }
    }
}