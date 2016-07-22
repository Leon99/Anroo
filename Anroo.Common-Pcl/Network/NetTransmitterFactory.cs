using System;
using System.Net;
using System.Net.Sockets;

namespace Anroo.Common.Network
{
    public static class NetTransmitterFactory
    {
        public static INetTransmitter Create(ProtocolType proto, IPEndPoint remoteEP, IPEndPoint localEP = null)
        {
            switch (proto)
            {
                case ProtocolType.Tcp:
                    return new TcpTransmitter(remoteEP, localEP);
                case ProtocolType.Udp:
                    return new UdpTransmitter(remoteEP, localEP);
                default:
                    throw new ArgumentOutOfRangeException(nameof(proto), proto, null);
            }
        }
    }
}