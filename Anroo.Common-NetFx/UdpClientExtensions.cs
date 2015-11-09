using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.Common
{
    public static class UdpClientExtensions
    {
        public static async Task SendAsync(this UdpClient client, byte[] datagram, IPEndPoint endPoint = null)
        {
            await client.SendAsync(datagram, datagram.Length, endPoint);
        }

        public static async Task SendRepeatedAsync(this UdpClient client, byte[] datagram, int repeatNumber, IPEndPoint endPoint = null)
        {
            for (int i = 0; i < repeatNumber; i++)
            {
                await client.SendAsync(datagram, endPoint);
            }
        }
    }
}