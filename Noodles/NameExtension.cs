using System.Collections.Generic;
using System.Linq;

namespace Noodles
{
    public delegate string ResolveName(object child);

    public interface IHasName
    {
        string Name { get; }
    }

    public static class NameExtension
    {
        static NameExtension()
        {
            NameRules = new List<Noodles.ResolveName>()
            {
                GetNameFromInterface
            };
        }

        public static Noodles.ResolveName GetNameFromInterface = o =>
        {
            var hasName = o as IHasName;
            if (hasName != null)
            {
                return (hasName).Name;
            }
            return null;
        };

        public static List<Noodles.ResolveName> NameRules;


        public static string Name(this object obj)
        {
            return NameRules.Select(r => r(obj)).FirstOrDefault(name => name != null);
        }
    }
}