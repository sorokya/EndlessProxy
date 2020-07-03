using System.Net;
using System.Net.Sockets;

namespace EndlessProxy
{
    class Server
    {
        private TcpListener listener;

        public void Start()
        {
            listener.Start();
        }

        public TcpClient Poll()
        {
            return listener.AcceptTcpClient();
        }

        public Server()
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8078);
        }
    }
}
