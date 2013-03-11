using System;
using System.Reflection;

namespace Noodles.Models
{
    public class NodeLink 
    {
        public INode Node { get; set; }

        public NodeLink(Resource parent, PropertyInfo pi)
        {
            var node = Resource.CreateGeneric(pi.GetValue(parent.Value), parent, pi.Name);
            Node = node;
            DisplayName = node.DisplayName;
            Url = node.Url;
            Name = pi.Name;
            TargetType = node.ValueType;
            Target = node;
        }
 

        public string Fragment { get; private set; }
        public string DisplayName { get; set; }

        public Type TargetType { get; set; }

        public string Url { get; private set; }
        public INode Parent { get; private set; }

        public string Name { get; set; }

        public INode Target { get; set; }
    }
}