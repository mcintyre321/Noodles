using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public interface IHasName
    {
        string Name { get; }
    }
    public interface IHasChildren
    {
        object GetChild(string name);        
    }
    public static class ChildrenExtensions
    {
        public static object GetChild(this object o, string name)
        {
            if (o is IHasChildren)
            {
                return ((IHasChildren) o).GetChild(name);
            }
            return null;
        }
    }

    public interface IHasPath
    {
        string Path { get; }
    }

    public delegate object ResolveParent(object child);
    public static class PathExtension
    {
        public static ResolveParent GetParentFromInterface = o =>
        {
            var hasParent = o as IHasParent<object>;
            if (hasParent != null)
            {
                return (hasParent).Parent;
            }
            return null;
        };

        public static List<ResolveParent> ParentRules = new List<ResolveParent>()
                                                            {
                                                                GetParentFromInterface
                                                            };


        public static string Path(this object obj)
        {
            if (obj is IHasPath) return ((IHasPath) obj).Path;
            if (obj is IHasName)
            {
                var node = (IHasName) obj;
                foreach (var resolveParent in ParentRules)
                {
                    var parent = resolveParent(node);
                    if (parent != null)
                        return parent.Path() + node.Name + "/";
                }
                
                return "/" + node.Name + "/";
            }
            return "/";
        }
    }

    public interface IHasParent<out T>
    {
        T Parent { get; }
    }

    public interface IHasId
    {
        string Id { get; }
    }
    public static class IdExtension
    {
        public static string Id(this object node)
        {
            if (node is IHasId) return ((IHasId)node).Id;
            var path = node.Path();
            return path.Replace('/', '_').Replace('@', '_').Replace(".", "_").Replace(' ', '_');
        }

        public static string Id(this INodeMethod method)
        {
            return method.Target.Id() + "_" + method.Name;
        }
        public static string Id(this NodeMethodParameter parameter)
        {
            return parameter.NodeMethod.Id() + "_" + parameter.Name;
        }

    }
    public interface IHasNodeType
    {
        Type NodeType { get; }
    }
    public static class NodeTypeExtension
    {
        public static Type NodeType(this object node)
        {
            if (node is IHasNodeType) return ((IHasNodeType)node).NodeType;
            return node.GetType();
        }
    }

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