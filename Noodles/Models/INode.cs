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
        string TypeName { get; }
        IEnumerable<NodeMethod> NodeMethods { get; }
        IEnumerable<NodeProperty> NodeProperties { get; }
        IEnumerable<INode> Children { get; }
    }
}