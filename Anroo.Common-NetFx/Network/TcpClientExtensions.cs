using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public static class TcpClientExtensions
    {
        public static void Connect(this TcpClient client, IPEndPoint endPoint)
        {
            client.Connect(endPoint.Address, endPoint.Port);
        }

        public static Task ConnectAsync(this TcpClient client, IPEndPoint endPoint)
        {
            return client.ConnectAsync(endPoint.Address, endPoint.Port);
        }
    }
}