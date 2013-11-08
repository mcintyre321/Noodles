using System;
using Noodles.Models;
using Walkies;

namespace Noodles.RequestHandling
{
    public static class NoodlesContext
    {
        public static Action<string, object> SetValue { get; set; }
        public static Func<string, object> GetValue { get; set; }

    }
    public static class NoodlesContextExtension
    {
        public static Resource GetResource(object target)
        {
            var resource = NoodlesContext.GetValue("Resource-" + target.GetHashCode()) as Resource;
            if (resource == null)
            {
                var parent = target.Parent();
                if (parent == null) throw new Exception("Could not get parent for " + target.ToString() + " - this object should have a parent findable through walkies");
                var parentResource = GetResource(parent);
                if (parentResource == null) throw new Exception("Could not get parent resource for " + target.ToString() + " - Noodles should have set the resource");
                var fragment = NoodlesContext.GetValue("Slug-" + target.GetHashCode()) as string;
                if (fragment == null) throw new Exception("Could not get parent resource for " + target.ToString());
                resource = parentResource.GetChild(fragment);
            }
            return resource;
        }

        public static Resource<T> GetResource<T>(T target)
        {
            return GetResource((object)target) as Resource<T>;
        }
    }
}