using System;

namespace Noodles.AspMvc.Helpers
{
    public static class StringExtensions
    {
        public static string RemoveFromStart(this string s, string value, bool ignoreCase = false)
        {
            return s == null || !s.StartsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ? s : s.Substring(value.Length);
        }
        public static string RemoveFromEnd(this string s, string value, bool ignoreCase = false)
        {
            return s == null || !s.EndsWith(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) ? s : s.Substring(0, s.LastIndexOf(value, StringComparison.Ordinal));
        }
    }
}