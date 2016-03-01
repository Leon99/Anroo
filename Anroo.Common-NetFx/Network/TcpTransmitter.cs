using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public class TcpTransmitter : NetTransmitter<TcpClient>
    {
        private NetworkStream _stream;
        private readonly object _connectionLock = new object();

        public TcpTransmitter(IPEndPoint remoteEP, IPEndPoint localEP = null) : base(remoteEP, localEP)
        {
            EnsureConnection();
        }

        public override Task SendDataAsync(byte[] data)
        {
            EnsureConnection();

            return _stream.WriteAsync(data);
        }

        private void EnsureConnection()
        {
            lock (_connectionLock)
            {
                if (!NetClient.Connected)
                {
                    NetClient.Connect(RemoteEP);
                    _stream = NetClient.GetStream();
                }
            }
        }

        public void Close()
        {
            _stream.Close();
            NetClient?.Close();
        }

        public override void Dispose()
        {
            Close();
        }
    }
}