using System;
using System.Collections.Generic;

namespace WebNoodle
{
    public static class NodeHelper
    {
        public static IEnumerable<INode> YieldChildren(this INode node, string path, bool breakOnNull = false)
        {
            yield return node;
            var parts = (path).Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                node = node.GetChild(part);
                if (node == null)
                {
                    if (breakOnNull) yield break;
                    throw new Exception("Node '" + part + "' not found in path '" + path + "'");
                }
                yield return node;
            }
        }
    }
}