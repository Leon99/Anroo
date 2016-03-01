using System;
using System.Net;
using System.Net.Sockets;

namespace Anroo.Common.Network
{
    public static class NetClientFactory
    {
        public static T CreateInstance<T>(IPEndPoint localEP = null) where T : class, new()
        {
            if (localEP == null)
            {
                return new T();
            }
            switch (typeof (T).Name)
            {
                case nameof(TcpClient):
                    return new TcpClient(localEP) as T;
                case nameof(UdpClient):
                    return new UdpClient(localEP) as T;
            }
            throw new ArgumentException();
        }
    }
}