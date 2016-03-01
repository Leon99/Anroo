using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Anroo.Common;
using Anroo.Common.Network;

namespace Anroo.OrviboS20
{
    public class OrviboS20Manager
    {
        public static async Task<IEnumerable<OrviboS20DiscoverCommandResponse>> DiscoverUnknownAsync(IPAddress localIP = null)
        {
            var discoveredThings = new Dictionary<int, OrviboS20DiscoverCommandResponse>();
            using (var transceiver = CreateTransceiver(localIP))
            {
                await transceiver.SendDataAsync(MessageFactory.CreateDiscoverUnknownMessage());
                await transceiver.ReceiveUntilAsync(result => 
                {
                    OrviboS20DiscoverCommandResponse discoverCommandResponse;
                    if (!TryParseDiscoverResponse(result, out discoverCommandResponse))
                    {
                        return false;
                    }
                    var macInt = BitConverter.ToInt32(discoverCommandResponse.NetworkAddress.MacAddress.GetRange(2), 0);
                    if (!discoveredThings.ContainsKey(macInt))
                    {
                        discoveredThings.Add(macInt, discoverCommandResponse);
                    }
                    return false;
                });
            }
            return discoveredThings.Select(pair => pair.Value);
        }


        public static async Task<OrviboS20DiscoverCommandResponse> DiscoverKnownAsync(byte[] mac, IPAddress localIP = null)
        {
            OrviboS20DiscoverCommandResponse discoverCommandResponse = null;
            using (var transceiver = CreateTransceiver(localIP))
            {
                await transceiver.SendDataAsync(MessageFactory.CreateDiscoverUnknownMessage());
                await transceiver.ReceiveUntilAsync(result => TryParseDiscoverResponse(result, out discoverCommandResponse) 
                                                             && discoverCommandResponse.NetworkAddress.MacAddress.SequenceEqual(mac));
            }
            return discoverCommandResponse;
        }

        private static UdpTransceiver CreateTransceiver(IPAddress localIP)
        {
            return new UdpTransceiver(
                new IPEndPoint(IPAddress.Broadcast, OrviboS20Ports.ThingPort), 
                new IPEndPoint(localIP, OrviboS20Ports.ControllerPort));
        }


        private static bool TryParseDiscoverResponse(UdpReceiveResult result, out OrviboS20DiscoverCommandResponse discoverCommandResponse)
        {
            OrviboS20CommandResponse parsedResponse;
            if (!OrviboS20CommandResponseFactory.TryParse(result, out parsedResponse))
            {
                discoverCommandResponse = null;
                return false;
            }
            discoverCommandResponse = parsedResponse as OrviboS20DiscoverCommandResponse;
            if (discoverCommandResponse == null)
            {
                return false;
            }
            return true;
        }
    }
}