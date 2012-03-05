using System;
using System.Collections.Generic;
using System.Web;

namespace WebNoodle
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
                node = node.GetChild(part);
                if (node == null)
                {
                    if (breakOnNull) yield break;
                    throw new HttpException(404, "Node '" + part + "' not found in path '" + path + "'");
                }
                yield return node;
            }
        }
    }
}