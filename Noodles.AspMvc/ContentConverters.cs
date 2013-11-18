using System;
using System.Collections.Generic;
using Noodles.Models;

namespace Noodles.AspMvc
{
    public static class ContentConverters
    {
        static readonly Dictionary<string, Func<INode, object>> Converters = new Dictionary<string, Func<INode, object>>();
        public static Func<INode, object> Get(string targetType)
        {
            return Converters.ContainsKey(targetType) ? Converters[targetType] : null;
        }
        public static void Set(string targetType, Func<INode, object> converter)
        {
            Converters[targetType] = converter;
        }
    }
}