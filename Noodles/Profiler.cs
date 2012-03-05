using System;

namespace Noodles
{
    public static class Profiler
    {
        public static Func<string, IDisposable> DoStep { get; set; }
        public static IDisposable Step(string message)
        {
            if (DoStep != null) DoStep(message);
            return new DummyDisposable();
        }

        public class DummyDisposable : IDisposable
        {
            public void Dispose()
            {
                
            }
        }
    }
}