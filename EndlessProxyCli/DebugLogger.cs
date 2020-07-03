using System;
using EndlessProxy;

namespace EndlessProxyCli
{
    class DebugLogger : IProxyDebugLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
