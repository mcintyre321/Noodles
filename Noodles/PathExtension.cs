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
        static ConditionalWeakTable<object, object> urlRoots = new ConditionalWeakTable<object, object>();

        public static T SetUrlRoot<T>(this T root, string urlRoot)
        {
            urlRoots.Remove(root);
            urlRoots.GetValue(root, r => urlRoot);
            return root;
        }
        public static T SetUrlRoot<T>(this T root, Func<string> getUrlRoot)
        {
            urlRoots.Remove(root);
            urlRoots.GetValue(root, r => getUrlRoot);
            return root;
        }
        public static string GetUrlRoot<T>(this T root)
        {
            var value = urlRoots.GetValue(root, r => null);
            if (value as Func<string> != null) return ((Func<string>) value)();
            return value as string;
        }

        public static string Path(this object o)
        {
            return o.WalkedPath("/");
        }

        public static string Url(this object o)
        {
            var walked = o.Walked();
            var rootPrefix = walked.First().GetUrlRoot() ?? "";
            if (!rootPrefix.EndsWith("/")) rootPrefix += "/";
            if (!rootPrefix.StartsWith("/")) rootPrefix = "/" + rootPrefix;

            return rootPrefix + String.Join("/", o.Walked().Skip(1).Select(w => w.GetFragment()));
        }
        
    }
}
