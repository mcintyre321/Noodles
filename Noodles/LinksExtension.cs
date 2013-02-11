using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodles
{
    public static class LinksExtension
    {
        public static IEnumerable<NodeLink> Links(this object obj)
        {
            yield break;
        } 
    }

    public class NodeLink
    {
    }
}
