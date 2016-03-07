﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public class UdpTransceiver : UdpTransmitter
    {
        public static readonly IPAddress DefaultLocalIP = IPAddress.Any;

        public UdpTransceiver(IPEndPoint remoteEP, IPAddress localIP = null) : this(remoteEP, localIP != null ? new IPEndPoint(localIP, 0) : null) { }
        public UdpTransceiver(IPEndPoint remoteEP, IPEndPoint localEP = null) : base(remoteEP, localEP)
        {
            NetClient.Client.ReceiveTimeout = 1000;
        }
        public async Task<UdpReceiveResult> ReceiveAsync()
        {
            for (var i = 0; i < 3; i++)
            {
                if (NetClient.Available > 0)
                {
                    IPEndPoint remoteEP = null;
                    var buffer = NetClient.Receive(ref remoteEP);
                    return new UdpReceiveResult(buffer, remoteEP);
                }
                await DelayAsync(500);
            }
            return default(UdpReceiveResult);
        }

        public async Task ReceiveUntilAsync(Func<UdpReceiveResult, bool> untilCheckFunc)
        {
            while (true)
            {
                var result = await ReceiveAsync();
                if (result.Buffer == null)
                {
                    break;
                }
                if (untilCheckFunc(result))
                {
                    while (NetClient.Available > 0)
                    {
                        // clear receive buffer
                        IPEndPoint remoteEP = null;
                        NetClient.Receive(ref remoteEP);
                    }
                    break;
                }
            }
        }
    }
}