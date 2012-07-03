using System.Web.Mvc;

namespace Noodles.Helpers
{
    public static class AttemptedValueHelper
    {
        public static string AttemptedValue<T>(this ViewDataDictionary<T> vd, string name)
        {
            if (vd.ModelState == null) return null;
            if (vd.ModelState.ContainsKey(name))
            {
                var modelState = vd.ModelState[name];
                if (modelState == null || modelState.Value == null) return null;
                return modelState.Value.AttemptedValue;
            }
            return null;
        }
    }
}