using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.Common.Udp
{
    public static class UdpClientExtensions
    {
        public static async Task SendAsync(this UdpClient client, byte[] datagram, IPEndPoint endPoint = null)
        {
            await client.SendAsync(datagram, datagram.Length, endPoint);
        }
    }
}