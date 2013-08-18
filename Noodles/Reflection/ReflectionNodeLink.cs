using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Helpers;
using Noodles.RequestHandling;

namespace Noodles.Models
{
    public class ReflectionNodeLink : NodeLink
    {
        public INode Node { get; set; }

        public ReflectionNodeLink(Resource parent, string name, object value, IEnumerable<Attribute> customAttributes)
        {
            Attributes = customAttributes;
            var node = ResourceFactory.Instance.Create(value, parent, name);
            Node = node;
            DisplayName = ((INode) node).DisplayName;
            Url = node.Url;
            Name = name;
            var uiHint = customAttributes.OfType<LinkAttribute>().Select(l => l.UiHint).SingleOrDefault()
                ?? customAttributes.OfType<LinksAttribute>().Select(l => l.UiHint).Single();
            UiHint = parent.Value.ToString(uiHint);
            ValueType = node.ValueType;
            Target = node;
        }


        public IEnumerable<INode> ChildNodes { get { yield break; } }
        public INode GetChild(string name)
        {
            return null;
        }

        public string DisplayName { get; set; }

        public Type ValueType { get; set; }

        public Uri Url { get; private set; }
        public IEnumerable<Attribute> Attributes { get; private set; }
        public INode Parent { get; private set; }

        public string Name { get; private set; }

        public Resource Target { get; set; }

        public string UiHint { get; private set; }
    }
}