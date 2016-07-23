using System.Text.RegularExpressions;

namespace Anroo.MiLight
{
    internal class DiscoverCommandResponse
    {
        public string IP;
        public string Mac;

        private static readonly Regex DiscoverResponseRegex = new Regex(
            $@"^(?<{DiscoverResponseIpGroupName}>\b\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\b),.*(?<{DiscoverResponseMacGroupName}>[0-9a-zA-Z]{{12}}).*$");

        private const string DiscoverResponseIpGroupName = "ip";
        private const string DiscoverResponseMacGroupName = "mac";

        internal static bool TryParse(string response, out DiscoverCommandResponse parsedResponse)
        {
            parsedResponse = new DiscoverCommandResponse();
            var ma = DiscoverResponseRegex.Match(response);
            if (!ma.Success)
            {
                return false;
            }
            parsedResponse.Mac = ma.Groups[DiscoverResponseMacGroupName].Value;
            parsedResponse.IP = ma.Groups[DiscoverResponseIpGroupName].Value;
            return true;
        }

    }
}