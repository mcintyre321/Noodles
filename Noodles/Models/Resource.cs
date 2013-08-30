using System;
using System.Collections;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface Resource : INode, IInvokeable
    {
    }
    public interface Resource<T> : Resource, INode<T>
    {
        new T Target { get; }
    }
}