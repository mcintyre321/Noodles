using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public interface IHasNodeMethods
    {
        IEnumerable<INodeMethod> NodeMethods();
    }
    public static class NodeMethodsExtensions
    {
        public static IEnumerable<INodeMethod> NodeMethods(this object o)
        {
            if (o is IHasNodeMethods) return ((IHasNodeMethods) o).NodeMethods();
            return o.GetNodeMethods();
        }
        public static INodeMethod NodeMethod(this object o, string methodName)
        {
            return o.NodeMethods().SingleOrDefault(m => m.Name == methodName);
        }
    }
}