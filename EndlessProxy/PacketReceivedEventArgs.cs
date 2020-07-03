using System;

namespace EndlessProxy
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public Packet packet;

        public PacketReceivedEventArgs(Packet packet)
        {
            this.packet = packet;
        }
    }
}
