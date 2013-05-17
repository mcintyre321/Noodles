using System;

namespace Noodles.Models
{
    public interface Resource : INode, IInvokeable, IHasNodeMethods, IHasNodeProperties, IHasNodeLinks
    {
        object Value { get; }
        Type ValueType { get; }
        
        Uri RootUrl { set; }
    }
    public interface Resource<T> : Resource, INode<T>
    {
        new T Target { get; }
    }
}