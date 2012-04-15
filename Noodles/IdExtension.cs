using System;

namespace Noodles
{
    public static class IdExtension
    {
        public static Func<object, string> MakeId { get; set; } 
        static IdExtension()
        {
            MakeId = ReplaceBadCharsWithUnderscores;
        }

        private static readonly Func<object, string> ReplaceBadCharsWithUnderscores = (object node) =>
        {
            var path = node.Path();
            return path.Replace('/', '_').Replace('@', '_').Replace(".", "_").Replace(' ', '_');
        };

        public static string Id(this object node)
        {
            return MakeId(node);
        }
    }
}