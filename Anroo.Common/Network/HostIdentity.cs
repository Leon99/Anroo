using System.Net;

namespace Anroo.Common.Network
{
    // TODO move to PCL after retargeting to NetFX 4.6
    public class HostIdentity
    {
        public IPAddress IPAddress { get; set; }
        public byte[] MacAddress { get; set; }
    }
}