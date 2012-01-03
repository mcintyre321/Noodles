using System;
using System.Collections.Generic;
using System.Linq;

namespace WebNoodle
{
    public static class NodeHelper
    {
        public static IEnumerable<INode> YieldChildren(this INode node, string path, bool breakOnNull = false)
        {
            var parts = (path).Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            using (var enumerator = parts.GetEnumerator())
            {
                yield return node;
                while (true)
                {
                    node = node.GetChild(enumerator);
                    if (node == null)
                    {
                        if (breakOnNull) yield break;
                        throw new Exception("Node '" + path + "' not found in path '" + path + "'");
                    }
                }
            }

        }
    }
}