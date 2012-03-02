using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebNoodle
{
    public class Logger
    {
        static Logger()
        {
            Trace = s => System.Diagnostics.Trace.WriteLine(s);
        }
        public static Action<string> Trace { get; set; }
    }
}
