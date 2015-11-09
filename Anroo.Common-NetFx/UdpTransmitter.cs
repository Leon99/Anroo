using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Anroo.Common
{
    public class UdpTransmitter : IDisposable
    {
        public const int DefaultDelayAfterSending = 110;
        public const int DelayAfterSending = DefaultDelayAfterSending;
        public int RepeatNumber { get; set; } = 1;

        protected UdpClient UdpClient;
        private readonly IPEndPoint _remoteEP;
        private readonly bool broadcasting;

        public UdpTransmitter(IPEndPoint remoteEP, IPEndPoint localEP = null)
        {
            _remoteEP = remoteEP;
            UdpClient = localEP != null 
                ? new UdpClient(localEP) 
                : new UdpClient();
            if (remoteEP != null)
            {
                broadcasting = Equals(remoteEP.Address, IPAddress.Broadcast);
                if (broadcasting)
                {
                    UdpClient.EnableBroadcast = true;
                    UdpClient.Client.MulticastLoopback = false;
                }
                else
                {
                    UdpClient.Connect(remoteEP);
                }
            }
        }

        public async Task SendDataAsync(string data)
        {
            await SendDataAsync(Encoding.UTF8.GetBytes(data));
        }

        public async Task SendDataAsync(byte[] data)
        {
            for (int i = 0; i < RepeatNumber; i++)
            {
                await UdpClient.SendAsync(data, broadcasting ? _remoteEP : null);
            }
            await DelayAsync();
        }

        public void Close()
        {
            UdpClient?.Close();
        }

        public async Task DelayAsync(int? duration = null)
        {
            if (duration > 0)
            {
                await Task.Delay(duration.GetValueOrDefault(DelayAfterSending));
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}