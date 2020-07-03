using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;


namespace EndlessProxy
{
    public class Proxy : IDisposable
    {
        private IProxyDebugLogger debugLogger;
        private Server server;
        private Socket localSocket, remoteSocket;

        private void DebugLog(string message)
        {
            if (debugLogger != null)
            {
                debugLogger.Log(message);
            }
        }

        public void Start()
        {
            server.Start();
            DebugLog("Proxy started.. listening for connection");
        }

        public void AcceptConnection()
        {
            localSocket = new Socket(server.Poll());
            localSocket.PacketReceived += LocalSocket_PacketReceived;
            DebugLog("Client connected.");
            remoteSocket = new Socket(new TcpClient("richardleek.com", 8079));
            remoteSocket.PacketReceived += RemoteSocket_PacketReceived;
            DebugLog("Connection to richardleek.com:8079 opened");

            var localThread = new Thread(new ThreadStart(LocalSocketTick));
            var remoteThread = new Thread(new ThreadStart(RemoteSocketTick));
            localThread.Start();
            remoteThread.Start();
        }

        private void RemoteSocket_PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            DebugLog("Remote: " + e.packet.ToString());
            var buffer = new List<byte>();
            buffer.AddRange(EONumber.Encode(e.packet.Data.Length, 2));
            buffer.AddRange(e.packet.Data);
            localSocket.Send(buffer.ToArray());
        }

        private void LocalSocket_PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            DebugLog("Local: " + e.packet.ToString());
            var buffer = new List<byte>();
            buffer.AddRange(EONumber.Encode(e.packet.Data.Length, 2));
            buffer.AddRange(e.packet.Data);
            remoteSocket.Send(buffer.ToArray());
        }

        private void LocalSocketTick()
        {
            while (true)
            {
                localSocket.Tick();
            }
        }

        private void RemoteSocketTick()
        {
            while (true)
            {
                remoteSocket.Tick();
            }
        }

        public Proxy()
        {
            server = new Server();
        }

        public Proxy(IProxyDebugLogger debugLogger) : this()
        {
            this.debugLogger = debugLogger;
        }


        public void Dispose()
        {
            foreach (IDisposable disposable in new IDisposable[] { localSocket, remoteSocket })
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

    }
}
