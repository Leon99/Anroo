using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Anroo.Common
{
    public class UdpTransceiver : UdpTransmitter
    {
        public static readonly IPAddress DefaultLocalIP = IPAddress.Any;

        public UdpTransceiver(IPEndPoint remoteEP, IPAddress localIP = null) : this(remoteEP, localIP != null ? new IPEndPoint(localIP, 0) : null) { }
        public UdpTransceiver(IPEndPoint remoteEP, IPEndPoint localEP = null) : base(remoteEP, localEP)
        {
            UdpClient.Client.ReceiveTimeout = 3000;
        }
        public async Task<string> ReceiveStringAsync()
        {
            for (var i = 0; i < 3; i++)
            {
                if (UdpClient.Available > 0)
                {
                    IPEndPoint remoteEP = null;
                    return Encoding.UTF8.GetString(UdpClient.Receive(ref remoteEP));
                }
                await DelayAsync(500);
            }
            return null;
        }
    }
}
