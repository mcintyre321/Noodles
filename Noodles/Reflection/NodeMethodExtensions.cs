using System;
using System.Collections.Generic;
using System.Linq;
using Noodles.Models;

namespace Noodles
{
    public static class NodeMethodExtensions
    {
        public static IEnumerable<NodeMethod> GetNodeMethods(this object o, INode resource)
        {
            return NodeMethodsReflectionLogic.YieldFindNodeMethodsUsingReflection(o, resource)
                .Where(nm => !nm.Name.StartsWith("set_")).Where(nm => !nm.Name.StartsWith("get_"));
        }
        
    }
}