using System;
using System.Collections.Generic;
using System.Web;

namespace Noodles
{
    public static class NodeHelper
    {
        public static IEnumerable<object> YieldChildren(this object node, string path, bool breakOnNull = false)
        {
            path = string.IsNullOrWhiteSpace(path) ? "/" : path;
            yield return node;
            var parts = (path).Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                if (part == "actions")
                {
                    node = new NodeActions(node);
                    yield return node;
                }
                else
                {
                    node = node.GetChild(part);
                    if (node == null)
                    {
                        if (breakOnNull) yield break;
                        throw new NodeNotFoundException("Node '" + part + "' not found in path '" + path + "'");
                    }
                    yield return node;
                }

            }
        }
    }
}