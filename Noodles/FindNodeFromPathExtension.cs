using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noodles
{
    public static class FindNodeFromPathExtension
    {
        public static List<FindNodeFromPathRule> Rules = new List<FindNodeFromPathRule>();

        public static object FindNodeFromPath(this object root, string path)
        {
            var node = Rules.Select(r => r(root, path)).FirstOrDefault(n => n != null);
            if (node == null)
            {
                throw new NodeNotFoundException("Node not found at path '" + path + "'");
            }
            return null;
        }

        public static FindNodeFromPathRule WalkChildren = (r, p) => r.YieldChildren(p).Last();




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
    public delegate object FindNodeFromPathRule(object root, string path);

}