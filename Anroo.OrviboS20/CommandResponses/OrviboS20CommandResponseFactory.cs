using System.Linq;
using System.Net.Sockets;
using Anroo.Common.Extensions;
using Anroo.Common.Network;

namespace Anroo.OrviboS20
{
    public class OrviboS20CommandResponseFactory
    {
        internal static bool TryParse(UdpReceiveResult udpResult, out OrviboS20CommandResponse response)
        {
            response = null;
            var code = udpResult.Buffer.GetRange(4, 2);
            //Trace.TraceInformation($"Parsing datagram of {udpResult.Buffer.Length} bytes from {udpResult.RemoteEndPoint.Address}, code is {code.ToCSharpString()}...");
            var length = udpResult.Buffer.Length;
            if (length == MessageTokens.PowerSwitchResponseLength && code.SequenceEqual(MessageTokens.PowerSwitchResponseCode))
            {
                response = ParsePowerStateResponse(udpResult);
                return true;
            }
            else if (length == MessageTokens.SubscribeResponseLength && code.SequenceEqual(MessageTokens.SubscribeCode))
            {
                response = ParseSubscribeResponse(udpResult);
                return true;
            }
            else if (length == MessageTokens.DiscoverKnownResponseLength
                     && (code.SequenceEqual(MessageTokens.DiscoverKnownResponseCode) || code.SequenceEqual(MessageTokens.DiscoverUnknownResponseCode)))
            {
                response = ParseDiscoverResponse(udpResult);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static OrviboS20DiscoverCommandResponse ParseDiscoverResponse(UdpReceiveResult udpResult)
        {
            var response = new OrviboS20DiscoverCommandResponse
            {
                NetworkAddress = new HostIdentity
                {
                    IPAddress = udpResult.RemoteEndPoint.Address,
                    MacAddress = udpResult.Buffer.GetRange(7, 6)
                },
                CommandId = udpResult.Buffer.GetRange(4, 3),
                IsPowerOn = udpResult.Buffer[41] == 1,
            };
            return response;
        }

        private static OrviboS20PowerStateResponse ParsePowerStateResponse(UdpReceiveResult udpResult)
        {
            int powerByteIndex = 22;
            return ParsePowerStateResponse<OrviboS20PowerStateResponse>(udpResult, powerByteIndex);
        }

        private static OrviboS20SubscribeResponse ParseSubscribeResponse(UdpReceiveResult udpResult)
        {
            return ParsePowerStateResponse<OrviboS20SubscribeResponse>(udpResult, 23);
        }

        private static T ParsePowerStateResponse<T>(UdpReceiveResult udpResult, int powerByteIndex) where T : OrviboS20PowerStateResponse, new()
        {
            var response = new T
            {
                NetworkAddress = new HostIdentity
                {
                    IPAddress = udpResult.RemoteEndPoint.Address,
                    MacAddress = udpResult.Buffer.GetRange(6, 6)
                },
                CommandId = udpResult.Buffer.GetRange(4, 2),
                IsPowerOn = udpResult.Buffer[powerByteIndex] == 1,
            };
            return response;
        }
    }
}