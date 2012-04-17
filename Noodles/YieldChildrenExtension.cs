using System;
using System.Collections.Generic;
using System.Web;

namespace Noodles
{
    public static class YieldChildrenExtension
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
                    node = new NodeMethods(node);
                    yield return node;
                }
                else
                {
                    var prev = node;
                    node = prev.GetChild(part);
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