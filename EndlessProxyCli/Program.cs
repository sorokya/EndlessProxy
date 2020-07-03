using System;
using EndlessProxy;

namespace EndlessProxyCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new Proxy(new DebugLogger());
            proxy.Start();
            proxy.AcceptConnection();
            Console.ReadLine();
        }
    }
}
