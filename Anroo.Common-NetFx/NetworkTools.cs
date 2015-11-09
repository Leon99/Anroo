using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Anroo.Common
{
    public static class NetworkTools
    {
        public struct LocalEndpointInformation
        {
            public string NetworkInterfaceName;
            public IPAddress UnicastAddress;

        }

        public static IEnumerable<LocalEndpointInformation> GetEligibleLocalEndpoints()
        {
            var nics = NetworkInterface.GetAllNetworkInterfaces();
            return 
                from nic in nics
                where nic.OperationalStatus == OperationalStatus.Up
                where nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
                where !nic.IsReceiveOnly
                where nic.SupportsMulticast
                from address in nic.GetIPProperties().UnicastAddresses
                where address.Address.AddressFamily == AddressFamily.InterNetwork
                select new LocalEndpointInformation
                {
                    UnicastAddress = address.Address,
                    NetworkInterfaceName = nic.Name
                };
        }
    }
}