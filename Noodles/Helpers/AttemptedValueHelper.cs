using System.Web.Mvc;

namespace Noodles.Helpers
{
    public static class AttemptedValueHelper
    {
        public static string AttemptedValue<T>(this ViewDataDictionary<T> vd, string name)
        {
            if (vd.ModelState.ContainsKey(name))
            {
                return vd.ModelState[name].Value.AttemptedValue;
            }
            return null;
        }
    }
}