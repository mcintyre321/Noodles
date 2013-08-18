using System;
using System.Collections;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface Resource : INode, IInvokeable, IHasNodeMethods, IHasNodeProperties, IHasNodeLinks
    {
        object Value { get; }
        Type ValueType { get; }
        
        Uri RootUrl { set; }
        IEnumerable<Attribute> CustomAttributes { get; }
    }
    public interface Resource<T> : Resource, INode<T>
    {
        new T Target { get; }
    }
}