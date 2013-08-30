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

  
}