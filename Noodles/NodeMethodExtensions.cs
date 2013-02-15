using System.Collections.Generic;
using System.Linq;
using Noodles.Requests;

namespace Noodles
{
    public static class NodeMethodExtensions
    {
        public static IEnumerable<NodeMethod> NodeMethods(this object o, Resource resource)
        {
            return NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(o, resource)
                .Where(nm => !nm.Name.StartsWith("set_"));
        }


        public static NodeMethod NodeMethod(this object o, string methodName, Resource resource)
        {
            return o.NodeMethods(resource).SingleOrDefault(m => m.Name.ToLowerInvariant() == methodName.ToLowerInvariant());
        }

        public static IEnumerable<IInvokeable> Actions(this object obj, Resource resource)
        {
            return obj.NodeMethods(resource).Cast<IInvokeable>()
                .Concat(obj.NodeProperties(resource).Where(p => !p.Readonly).Select(p => p.Setter));
        }
    }
}