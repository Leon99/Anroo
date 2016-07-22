using System;
using System.Net.NetworkInformation;

namespace Anroo.Common.Network
{
    public static class PhysicalAddressFactory
    {
        public static bool TryParse(string address, out PhysicalAddress mac)
        {
            try
            {
                mac = PhysicalAddress.Parse(address);
                return true;
            }
            catch (FormatException)
            {
                mac = null;
                return false;
            }
        }
    }
}
