using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Walkies;

namespace Noodles
{
    public static class PathExtension
    {
        static ConditionalWeakTable<object, string> urlRoots = new ConditionalWeakTable<object, string>();

        public static T SetUrlRoot<T>(this T root, string urlRoot)
        {
            urlRoots.Remove(root);
            urlRoots.GetValue(root, r => urlRoot);
            return root;
        }
        public static string GetUrlRoot<T>(this T root)
        {
            return urlRoots.GetValue(root, r => null) as string;
        }

        public static string Path(this object o)
        {
            return o.WalkedPath("/");
        }

        public static string Url(this object o)
        {
            var walked = o.Walked();
            var rootPrefix = walked.First().GetUrlRoot() ?? "/";
            return rootPrefix + String.Join("/", o.Walked().Select(w => w.GetFragment()));
        }
        
    }
}
