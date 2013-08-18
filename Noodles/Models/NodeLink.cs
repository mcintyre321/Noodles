using System;
using System.Collections.Generic;
using System.Reflection;

namespace Noodles.Models
{
    public interface NodeLink : INode
    {
        Resource Target { get; }
        Type ValueType { get; }
        string UiHint { get; }
    }
}