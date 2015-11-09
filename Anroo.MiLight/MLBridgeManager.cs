using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Anroo.Common;

namespace Anroo.MiLight
{
    public static class MLBridgeManager
    {
        internal const string DiscoverRequest = "Link_Wi-Fi";

        internal static readonly Regex DiscoverResponseRegex = new Regex(
                $@"^(?<{DiscoverResponseIpGroupName}>\b\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\b),.*(?<{DiscoverResponseMacGroupName}>[0-9a-zA-Z]{{12}}).*$");

        private const string DiscoverResponseIpGroupName = "ip";
        private const string DiscoverResponseMacGroupName = "mac";

        public static async Task<IEnumerable<HostIdentity>> DiscoverBridgesAsync(IPAddress localIP = null)
        {
            var bridges = new Dictionary<string, string>();
            using (var client = new UdpTransceiver(new IPEndPoint(IPAddress.Broadcast, 48899), localIP) {RepeatNumber = 10} )
            {
                await client.SendDataAsync(DiscoverRequest);
                string data;
                while (!String.IsNullOrEmpty(data = await client.ReceiveStringAsync()))
                {
                    var ma = DiscoverResponseRegex.Match(data);
                    if (!ma.Success)
                    {
                        continue;
                    }
                    var mac = ma.Groups[DiscoverResponseMacGroupName].Value;
                    if (!bridges.ContainsKey(mac))
                    {
                        bridges.Add(mac, ma.Groups[DiscoverResponseIpGroupName].Value);
                    }
                }
            }
            return bridges.Select(pair => new HostIdentity
            {
                IPAddress = IPAddress.Parse(pair.Value),
                MacAddress = ByteArrayHelpers.FromString(pair.Key)
            });
        }
    }
}