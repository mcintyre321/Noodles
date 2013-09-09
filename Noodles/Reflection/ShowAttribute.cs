using System;

namespace Noodles
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]

    public class ShowAttribute : Attribute
    {
        public ShowAttribute()
        {
        }
    }
     
}