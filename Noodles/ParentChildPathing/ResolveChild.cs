using System;

namespace Noodles
{
    public delegate object ResolveChild(object node, string childName);

    //public class ChildAttribute : Attribute
    //{
    //    private readonly string _name;
    //    private static List<AllowGetChildFromPropertyInfoRule> Rules;

    //    static ChildAttribute()
    //    {
    //        Rules = new List<AllowGetChildFromPropertyInfoRule>();
    //    }

    //    public ChildAttribute(){}
    //    public ChildAttribute(string name): this()
    //    {
    //        _name = name;
    //    }

    //    public object GetChild(object obj, PropertyInfo pi)
    //    {
    //        var allow = Rules.Select(r => r(obj, pi)).FirstOrDefault(r => r != null) ?? true;
    //        if (allow)
    //        {
    //            var child = pi.GetValue(obj, null);
    //            if (child.Name() == null)
    //            {
    //                child.SetName(_name ?? pi.Name);
    //            }
    //            return child;
    //        }
    //        return null;
    //    }
    //}
}