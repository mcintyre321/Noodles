namespace Noodles.AspMvc
{
    public static class HelperExtensions
    {
        public static string ToClassName(this string s)
        {
            return s.Replace("`1", "_");
        }
    }
}