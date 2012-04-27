using System;
using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public static class ParentChildPathRules 
    {

        public static readonly ResolvePathForNode WalkParents = obj =>
        {
            var name = obj.Name();
            if (name == null) return "/";
            var parent = obj.Parent();
            if (parent != null)
            {
                var parentPath = parent.Path();
                return parentPath + name + "/";
            }
            return name + "/";
        };

        public static ResolveNodeFromPath WalkChildren = (r, p) => r.YieldChildren(p).Last();


        private static IEnumerable<object> YieldChildren(this object node, string path)
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
                        yield return null; //could not find node, return null
                        yield break;
                    }

                    yield return node;
                }

            }
        }


    }
}