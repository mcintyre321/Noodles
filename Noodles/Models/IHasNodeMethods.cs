using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles
{
    public interface IHasName
    {
        string Name { get; }
    }
    public static class HasNameHelperExtensions
    {
        public static IEnumerable<T> ExceptNamed<T>(this IEnumerable<T> items, params string[] excludedNames) where T : IHasName
        {
            return items.Where(i => excludedNames.Contains(i.Name) == false);
        }
        public static IEnumerable<T> ExceptNamed<T>(this IEnumerable<T> items, IEnumerable<string> excludedNames) where T : IHasName
        {
            return items.ExceptNamed(excludedNames.ToArray());
        }

    }

    public interface IHasNodeMethods
    {
        IEnumerable<NodeMethod> NodeMethods { get; }
    }

    public interface IHasNodeProperties
    {
        IEnumerable<NodeProperty> NodeProperties { get; }
    }

    public interface IHasNodeLinks
    {
        IEnumerable<NodeLink> NodeLinks { get; }
    }

    public static class HasNodeHelperExtensions
    {
        public static NodeMethod NodeMethod(this IHasNodeMethods o, string methodName)
        {
            return o.NodeMethods.SingleOrDefault(n => StringComparer.InvariantCultureIgnoreCase.Compare(n.Name, methodName) == 0);
        }
        public static NodeProperty NodeProperty(this IHasNodeProperties o, string methodName)
        {
            return o.NodeProperties.SingleOrDefault(n => StringComparer.InvariantCultureIgnoreCase.Compare(n.Name, methodName) == 0);
        }
        public static NodeLink NodeLink(this IHasNodeLinks o, string LinkName)
        {
            return o.NodeLinks.SingleOrDefault(n => StringComparer.InvariantCultureIgnoreCase.Compare(n.Name, LinkName) == 0);
        }

    }
}