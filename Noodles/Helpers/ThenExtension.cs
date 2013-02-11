using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodles.Helpers
{
    static class ThenExtension
    {
        public static IEnumerable<T> Then<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
                yield return item;
            }
        }
    }
}
