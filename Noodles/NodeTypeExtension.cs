using System;

namespace Noodles
{
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
}