using System;
using System.Threading.Tasks;

namespace Anroo.Common.Network
{
    public interface INetTransmitter : IDisposable
    {
        Task SendDataAsync(byte[] data, int? sendRepeats = null);
    }
}