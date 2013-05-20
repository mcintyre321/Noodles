using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface NodeProperty : INode
    {
        string Name { get; }
        string DisplayName { get; }
        INode Parent { get; }
        int Order { get; }
        Type ValueType { get; }
        bool Readonly { get; }
        object Value { get; }
        IEnumerable<object> CustomAttributes { get; }
        object Invoke(object[] objects);
        IEnumerable<NodeMethod> NodeMethods { get; }
        string UiHint { get; }
    }
    public interface NodeCollectionProperty : NodeProperty
    {

        QueryPage Query(int skip, int take);
    }
}