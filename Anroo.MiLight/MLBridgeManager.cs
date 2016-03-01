using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Anroo.Common;
using Anroo.Common.Network;

namespace Anroo.MiLight
{
    public static class MLBridgeManager
    {
        internal const string DiscoverRequest = "Link_Wi-Fi";
        private const int BridgeControlPort = 48899;

        public static async Task<IEnumerable<HostIdentity>> DiscoverAsync(IPAddress controllerIP = null)
        {
            var discoveredThings = new Dictionary<string, string>();
            using (var transceiver = CreateTransceiver(controllerIP))
            {
                await transceiver.SendDataAsync(DiscoverRequest);
                await transceiver.ReceiveUntilAsync(result => ProcessReceiveResults(result, discoveredThings));
            }
            return discoveredThings.Select(pair => new HostIdentity
            {
                IPAddress = IPAddress.Parse(pair.Value),
                MacAddress = ByteArrayHelpers.FromString(pair.Key)
            });
        }

        private static bool ProcessReceiveResults(UdpReceiveResult result, Dictionary<string, string> discoveredThings)
        {
            var response = Encoding.UTF8.GetString(result.Buffer);
            DiscoverCommandResponse parsedResponse;
            if (!DiscoverCommandResponse.TryParse(response, out parsedResponse))
            {
                return false;
            }
            if (!discoveredThings.ContainsKey(parsedResponse.Mac))
            {
                discoveredThings.Add(parsedResponse.Mac, parsedResponse.IP);
            }
            return false;
        }

        private static UdpTransceiver CreateTransceiver(IPAddress controllerIP)
        {
            return new UdpTransceiver(new IPEndPoint(IPAddress.Broadcast, BridgeControlPort), controllerIP) {SendRepeats = 10};
        }
    }
}