using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface INode
    {
        INode GetChild(string fragment);
        string Fragment { get; }
        string DisplayName { get; }
        string Url { get; }
        INode Parent { get; }
        string UiHint { get; }
        object Value { get; }
        Type ValueType { get; }

        int Order { get; }
        IEnumerable<NodeMethod> NodeMethods { get; }
        IEnumerable<INode> NodeProperties { get; }
        IEnumerable<INode> Children { get; }
    }
}