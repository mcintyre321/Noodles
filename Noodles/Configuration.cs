using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noodles
{
    public static class Configuration
    {
        static Configuration()
        {
             //use Harden rules to control method access
            NodeMethodsRuleRegistry.ShowMethodRules.Add((o, info) => Harden.Allow.Call(o, info) ? null as bool? : false);
            NodeMethodsRuleRegistry.ShowMethodRules.Add((o, m) => m.Name.StartsWith("Allow") ? false : null as bool?);

            NodePropertiesRuleRegistry.ShowPropertyRules.Add((o, info) => Harden.Allow.Get(o, info) ? null as bool? : false);


            
        }
        
        public static void Initialise()
        {
        }
    }
}
