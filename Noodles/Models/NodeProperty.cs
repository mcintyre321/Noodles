using System;
using System.Collections.Generic;

namespace Noodles.Models
{
    public interface NodeProperty : INode, IInvokeableParameter
    {
        int Order { get; }
        IEnumerable<NodeMethod> NodeMethods { get; }
        string UiHint { get; }
        void SetValue(object value);
    }
    public interface NodeCollectionProperty : NodeProperty
    {

        QueryPage Query(int skip, int take);
    }
}