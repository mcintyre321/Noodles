using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Helpers;

namespace Noodles.Models
{
    public interface INode<T>
    {
    }

    public interface INode : IHasName
    {
        IEnumerable<INode> ChildNodes { get; }
        INode GetChild(string name);
        string DisplayName { get; }
        Uri Url { get; }
        INode Parent { get; }
        Type ValueType { get; }
        IEnumerable<Attribute> CustomAttributes { get; } 
        //string UiHint { get; }

        //int Order { get; }

    }
    public static class INodeExtensions
    {
        public static IEnumerable<INode> Ancestors(this INode t) {  { return t.AncestorsAndSelf().Skip(1); } }
        public static IEnumerable<INode> AncestorsAndSelf(this INode t) {  { return (t).Recurse(n => n.Parent); } }
    }
}