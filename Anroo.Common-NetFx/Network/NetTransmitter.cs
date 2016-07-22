using System;
using System.Net;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public abstract class NetTransmitter<T> : INetTransmitter where T : class, IDisposable, new()
    {
        protected T NetClient;
        protected IPEndPoint RemoteEP;

        protected NetTransmitter(IPEndPoint remoteEP, IPEndPoint localEP = null)
        {
            RemoteEP = remoteEP;
            NetClient = NetClientFactory.CreateInstance<T>(localEP);
        }

        public int SendRepeats { get; set; } = 1;

        public virtual async Task SendDataAsync(byte[] data, int? sendRepeats)
        {
            int effectiveSendRepeats = sendRepeats ?? SendRepeats;
            for (int i = 0; i < effectiveSendRepeats; i++)
            {
                await SendDataAsync(data);
            }
        }

        public abstract Task SendDataAsync(byte[] data);

        public virtual void Dispose()
        {
            NetClient.Dispose();
        }
    }
}