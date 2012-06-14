using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Walkies;

namespace Noodles
{
    public static class PathExtension
    {
        public static string Path(this object o)
        {
            return "/" + o.WalkPath("/");
        }
    }
}
