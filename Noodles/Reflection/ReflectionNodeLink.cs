using System;
using Noodles.Helpers;
using Noodles.RequestHandling;

namespace Noodles.Models
{
    public class ReflectionNodeLink : NodeLink
    {
        public INode Node { get; set; }

        public ReflectionNodeLink(Resource parent, string name, object value, string uiHint)
        {
            var node = ResourceFactory.Instance.Create(value, parent, name);
            Node = node;
            DisplayName = ((INode) node).DisplayName;
            Url = node.Url;
            Name = name;
            UiHint = parent.Value.ToString(uiHint);
            TargetType = node.ValueType;
            Target = node;
        }


        public string Fragment { get; private set; }
        public string DisplayName { get; set; }

        public Type TargetType { get; set; }

        public Uri Url { get; private set; }
        public INode Parent { get; private set; }

        public string Name { get; set; }

        public Resource Target { get; set; }

        public string UiHint { get; private set; }
    }
}