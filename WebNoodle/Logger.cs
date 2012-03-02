using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WebNoodle
{
    public class Logger
    {
        static Logger()
        {
            Trace = s => { };
            LogException = (s, e) => { };
        }
        public static Action<string> Trace { get; set; }

        public static Action<string, Exception> LogException { get; set; } 
    }
}
