using System.Diagnostics;
using System.Net;
using Should;
using Xunit;

namespace Anroo.OrviboS20.Tests
{
    [Trait("Category", "IgnoreForCI")]
    public class OrviboS20ControllerTests
    {

        [Fact]
        public void ComplexTest()
        {
            // Arrange
            var mac = new byte[] { 0xac, 0xcf, 0x23, 0x4b, 0x62, 0x28 };
            var ip = IPAddress.Parse("192.168.1.46");

            // Act
            Trace.TraceInformation("Initializing controller...");
            using (var device = new OrviboS20Controller(ip, mac))
            {
                DoPowerCycle(device);
                DoPowerCycle(device);
            }
        }

        private static void DoPowerCycle(OrviboS20Controller device)
        {
            Trace.TraceInformation("Sending power on command...");
            var result = device.PowerOnAsync().Result;
            result
                .ShouldBeType<OrviboS20PowerStateResponse>();
            result.IsPowerOn
                .ShouldBeTrue();

            Trace.TraceInformation("Sending power off command...");
            result = device.PowerOffAsync().Result;
            result
                .ShouldBeType<OrviboS20PowerStateResponse>();
            result.IsPowerOn
                .ShouldBeFalse();
        }
    }
}