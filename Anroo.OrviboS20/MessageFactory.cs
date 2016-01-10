using System.Linq;
using Anroo.Common;

namespace Anroo.OrviboS20
{
    internal class MessageFactory
    {
        private readonly byte[] _macAddress;

        public MessageFactory(byte[] macAddress)
        {
            _macAddress = macAddress;

        }

        public static byte[] CreateDiscoverUnknownMessage()
        {
            return ByteArrayHelpers.Combine(
                MessageTokens.CommandPrefix,
                MessageTokens.DiscoverUnknownRequestLength,
                MessageTokens.DiscoverUnknownRequestCode);
        }

        public byte[] CreateDiscoverKnownMessage()
        {
            return ByteArrayHelpers.Combine(
                MessageTokens.CommandPrefix,
                MessageTokens.DiscoverKnownRequestLength,
                MessageTokens.DiscoverKnownRequestCode,
                _macAddress,
                MessageTokens.MacAddressPadding);
        }

        public byte[] CreatePowerOnMessage()
        {
            return ByteArrayHelpers.Combine(
                MessageTokens.CommandPrefix,
                MessageTokens.PowerSwitchRequestLength,
                MessageTokens.PowerSwitchRequestCode,
                _macAddress,
                MessageTokens.MacAddressPadding,
                MessageTokens.EmptyPadding,
                new byte[] {0x01});
        }
        public byte[] CreatePowerOffMessage()
        {
            return ByteArrayHelpers.Combine(
                MessageTokens.CommandPrefix,
                MessageTokens.PowerSwitchRequestLength,
                MessageTokens.PowerSwitchRequestCode,
                _macAddress,
                MessageTokens.MacAddressPadding,
                MessageTokens.EmptyPadding,
                new byte[] {0x0});
        }

        public byte[] CreateSubscribeMessage()
        {
            var macAddressReversed = _macAddress.Reverse().ToArray();

            return ByteArrayHelpers.Combine(
                MessageTokens.CommandPrefix,
                MessageTokens.SubscribeRequestLength,
                MessageTokens.SubscribeCode,
                _macAddress,
                MessageTokens.MacAddressPadding,
                macAddressReversed,
                MessageTokens.MacAddressPadding);
        }
    }
}