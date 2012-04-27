using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Noodles
{
    public delegate object ResolveNodeFromPath(object root, string path);

    public static class ResolveNodeFromPathExtension
    {
        static ResolveNodeFromPathExtension()
        {
            Rules = new List<ResolveNodeFromPath>()
            {
                ParentChildPathRules.WalkChildren
            };
        }

        public static List<ResolveNodeFromPath> Rules;

        public static object FindNodeFromPath(this object root, string path)
        {
            var node = Rules.Select(r => r(root, path)).FirstOrDefault(n => n != null);
            if (node == null)
            {
                throw new NodeNotFoundException("Node not found at path '" + path + "'");
            }
            return node;
        }

    }


}