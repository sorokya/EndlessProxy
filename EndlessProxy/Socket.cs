using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace EndlessProxy
{
    class Socket : IDisposable
    {
        private TcpClient client;
        private NetworkStream stream;
        private ReadState readState;
        private int length;
        private List<byte> data;

        private byte[] rawLength;

        private byte[] Receive(int length)
        {
            var buffer = new byte[length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public void Send(byte[] buffer)
        {
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

        public void Tick()
        {
            // TODO: implement packet encoding/decoding!
            var done = false;
            int oldLength;
            var data = new List<byte>(Receive(readState == ReadState.ReadData ? length : 1));

            while (data.Count > 0 && !done)
            {
                switch (readState)
                {
                    case ReadState.ReadLength1:
                        {
                            rawLength[0] = data[0];
                            data.RemoveAt(0);
                            readState = ReadState.ReadLength2;

                            if (data.Count == 0)
                            {
                                break;
                            }
                            goto case ReadState.ReadLength2;
                        }
                    case ReadState.ReadLength2:
                        {
                            rawLength[1] = data[0];
                            data.RemoveAt(0);
                            length = (int)EONumber.Decode(rawLength);
                            readState = ReadState.ReadData;
                            if (data.Count == 0)
                            {
                                break;
                            }
                            goto case ReadState.ReadData;
                        }
                    case ReadState.ReadData:
                        {
                            oldLength = this.data.Count;
                            this.data.AddRange(data);
                            length -= this.data.Count - oldLength;

                            if (length == 0)
                            {
                                OnPacketReceived(new PacketReceivedEventArgs(new Packet(this.data.ToArray())));
                                this.data.Clear();
                                readState = ReadState.ReadLength1;
                                done = true;
                            }
                            break;
                        }
                    default:
                        done = true;
                        break;
                }
            }
        }

        protected virtual void OnPacketReceived(PacketReceivedEventArgs e)
        {
            PacketReceived?.Invoke(this, e);
        }

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;

        public Socket(TcpClient tcpClient)
        {
            client = tcpClient;
            stream = client.GetStream();
            rawLength = new byte[2];
            data = new List<byte>();
        }

        private enum ReadState
        {
            ReadLength1,
            ReadLength2,
            ReadData
        }

        public void Dispose()
        {
            foreach (IDisposable disposable in new IDisposable[] { stream, client })
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
