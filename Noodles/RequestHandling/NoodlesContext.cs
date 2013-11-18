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
                var fragment = target.GetFragment();
                if (fragment == null) throw new Exception("Couldn't find Resource for " + target + " - add the [Fragment] attribute to the appropriate property on the " + target.GetType() + " so that we can walk to it");
                
                var parent = target.Parent();
                if (parent == null) throw new Exception("Could not get parent for " + target.ToString() + " - this object should have a parent findable through walkies - add the [Parent] attribute");
         

                var parentResource = GetResource(parent);
                if (parentResource == null) throw new Exception("Could not get parent resource for " + target.ToString() + " - Noodles should have set the resource");
         
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