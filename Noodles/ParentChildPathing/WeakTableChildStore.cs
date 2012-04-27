using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Noodles.Helpers;

namespace Noodles
{
    public static class WeakTableChildStore
    {
        public static ResolveParent GetParentFromMetaProps = o => o.Meta()["Parent"];
        private static readonly ConditionalWeakTable<object, Hashtable> Props = new ConditionalWeakTable<object, Hashtable>();

        public static T SetParentInMeta<T>(this T child, object parent, string name = null)
        {
            name = name ?? "_";
            if (parent == null)
            {
                var currentParent = child.Parent();
                if (currentParent != null)
                {
                    currentParent.Children().Remove(child.Name());
                    child.SetName(null);
                }
                child.Meta().Remove("Parent");
                return child;
            }
            else
            {
                if (child.Name() != null) parent.Children().Remove(child.Name());

                child.Meta()["Parent"] = parent;
                

                child.SetName(MakeNameSafe(name, n => parent.GetChild(n) == null));
                parent.Children()[child.Name()] = child;
                return child;
            }
        }

        static string MakeNameSafe(string name, Func<string, bool> nameIsSafe)
        {
            var nameCounter = 0;
            var safeName = name;
            if (safeName == "_") safeName = "_0";
            while (!nameIsSafe(safeName))
            {
                nameCounter++;
                safeName = name + nameCounter;
            }
            return safeName;
        }


        private static Hashtable Meta(this object key)
        {
            return Props.GetOrCreateValue(key);
        }



        public static IEnumerable<object> Descendants(this object obj)
        {
            return obj.Recurse(o => Children(o).Values.Cast<object>());
        }


        public static Hashtable Children(this object obj)
        {
            var children = obj.Meta()["Children"] as Hashtable;
            if (children == null) obj.Meta()["Children"] = children = new Hashtable();
            return children;
        }

        public static object GetChildForObject(object node, string childName)
        {
            return node.Children()[childName];
        }
    }
}