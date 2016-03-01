using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Anroo.Common.Network
{
    public static class NetworkTools
    {
        public struct LocalEndpointInformation
        {
            public string NetworkInterfaceName;
            public IPAddress UnicastAddress;

        }

        public static IEnumerable<LocalEndpointInformation> GetEligibleLocalEndpoints() => 
            from nic in NetworkInterface.GetAllNetworkInterfaces()
            where nic.OperationalStatus == OperationalStatus.Up
            where nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
            where !nic.IsReceiveOnly
            where nic.SupportsMulticast
            where nic.Supports(NetworkInterfaceComponent.IPv4)
            from address in nic.GetIPProperties().UnicastAddresses
            where address.Address.AddressFamily == AddressFamily.InterNetwork
            select new LocalEndpointInformation
            {
                UnicastAddress = address.Address,
                NetworkInterfaceName = nic.Name
            };

        public static IPAddress GetLocalIPAddress(string interfaceName) => 
            NetworkInterface.GetAllNetworkInterfaces()
            .Single(nic => nic.Name == interfaceName)
            .GetIPProperties()
            .UnicastAddresses
            .First(addressInformation => addressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
            .Address;
    }
}