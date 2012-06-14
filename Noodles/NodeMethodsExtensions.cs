using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public interface IHasNodeMethods
    {
        IEnumerable<NodeMethod> NodeMethods();
    }
    public static class NodeMethodsExtensions
    {
        public static IEnumerable<NodeMethod> NodeMethods(this object o)
        {
            return new NodeMethods(o);
        }
        public static NodeMethod NodeMethod(this object o, string methodName)
        {
            return o.NodeMethods().SingleOrDefault(m => m.Name == methodName);
        }
    }
}