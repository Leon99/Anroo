using System;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Anroo.Common.Udp
{
    public class UdpTransmitter : IDisposable
    {
        public const int DefaultDelayAfterSending = 110;
        public int DelayAfterSend = DefaultDelayAfterSending;
        public int SendRepeats { get; set; } = 1;

        protected UdpClient UdpClient;
        private readonly IPEndPoint _remoteEP;
        private readonly bool _isBroadcast;

        public UdpTransmitter(IPEndPoint remoteEP, IPEndPoint localEP = null)
        {
            _remoteEP = remoteEP;
            UdpClient = localEP != null 
                ? new UdpClient(localEP)
                : new UdpClient();
            if (remoteEP != null)
            {
                _isBroadcast = Equals(remoteEP.Address, IPAddress.Broadcast);
                if (_isBroadcast)
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

        public async Task SendDataAsync(string data, int? sendRepeats = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Contract.EndContractBlock();

            await SendDataAsync(Encoding.UTF8.GetBytes(data));
        }

        public async Task SendDataAsync(byte[] data, int? sendRepeats = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            Contract.EndContractBlock();

            int effectiveSendRepeats = sendRepeats ?? SendRepeats;
            for (int i = 0; i < effectiveSendRepeats; i++)
            {
                await UdpClient.SendAsync(data, _isBroadcast ? _remoteEP : null);
                await DelayAsync(DelayAfterSend);
            }
        }

        public void Close()
        {
            UdpClient?.Close();
        }

        public async Task DelayAsync(int duration)
        {
            await Task.Delay(duration);
        }

        public void Dispose()
        {
            Close();
        }
    }
}