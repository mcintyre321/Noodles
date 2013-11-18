using System;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles.AspMvc
{
    public static class ContentConverters
    {
        static readonly Dictionary<string, Func<object, INode>> Converters = new Dictionary<string, Func<object, INode>>();
        public static Func<object, INode> Get(string targetType)
        {
            return Converters.ContainsKey(targetType) ? Converters[targetType] : null;
        }
        public static void Set(string targetType, Func<object, INode> converter)
        {
            Converters[targetType] = converter;
        }
    }
}