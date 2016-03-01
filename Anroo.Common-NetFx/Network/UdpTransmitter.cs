using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public class UdpTransmitter : NetTransmitter<UdpClient>
    {
        public int DelayAfterSend = DefaultDelayAfterSending;
        private const int DefaultDelayAfterSending = 110;

        private readonly bool _isBroadcast;

        public UdpTransmitter(IPEndPoint remoteEP, IPEndPoint localEP = null) : base(remoteEP, localEP)
        {
            if (remoteEP != null)
            {
                _isBroadcast = Equals(remoteEP.Address, IPAddress.Broadcast);
                if (_isBroadcast)
                {
                    NetClient.EnableBroadcast = true;
                    NetClient.Client.MulticastLoopback = false;
                }
                else
                {
                    NetClient.Connect(remoteEP);
                }
            }
        }

        public async Task SendDataAsync(string data, int? sendRepeats = null)
        {
            await SendDataAsync(Encoding.UTF8.GetBytes(data));
        }

        public override async Task SendDataAsync(byte[] data)
        {
            await NetClient.SendAsync(data, _isBroadcast ? RemoteEP : null);
            await DelayAsync(DelayAfterSend);
        }

        public void Close()
        {
            NetClient?.Close();
        }

        protected async Task DelayAsync(int duration)
        {
            await Task.Delay(duration);
        }

        public override void Dispose()
        {
            Close();
        }
    }
}