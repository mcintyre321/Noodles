using System;
using System.Reflection;

namespace Noodles.Models
{
    public class NodeLink
    {
        public INode Node { get; set; }

        public NodeLink(Resource parent, string name, object value, string uiHint)
        {
            var node = Resource.CreateGeneric(value, parent, name);
            Node = node;
            DisplayName = node.DisplayName;
            Url = node.Url;
            Name = name;
            UiHint = uiHint;
            TargetType = node.ValueType;
            Target = node;
        }


        public string Fragment { get; private set; }
        public string DisplayName { get; set; }

        public Type TargetType { get; set; }

        public string Url { get; private set; }
        public INode Parent { get; private set; }

        public string Name { get; set; }

        public Resource Target { get; set; }

        public string UiHint { get; private set; }
    }
}