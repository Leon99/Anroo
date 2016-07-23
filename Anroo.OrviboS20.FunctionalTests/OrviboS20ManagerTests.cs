using System.Linq;
using System.Net;
using Anroo.Common.Network;
using Should;
using Xunit;

namespace Anroo.OrviboS20.Tests
{
    [Trait("Category", "IgnoreForCI")]
    public class OrviboS20ManagerTests
    {
        private static readonly IPAddress LocalIp = NetworkTools.GetLocalIPAddress("Wi-Fi");

        [Fact]
        public void DiscoverUnknown()
        {
            var discoveredAddresses = OrviboS20Manager.DiscoverUnknownAsync(LocalIp).Result;

            discoveredAddresses.Any()
                .ShouldBeTrue();
        }

        [Fact]
        public void DiscoverKnown()
        {
            var mac = new byte[] { 0xac, 0xcf, 0x23, 0x4b, 0x62, 0x28 };
            var discoveredAddresses = OrviboS20Manager.DiscoverKnownAsync(mac, LocalIp).Result;

            discoveredAddresses
                .ShouldNotBeNull();
        }
    }
}