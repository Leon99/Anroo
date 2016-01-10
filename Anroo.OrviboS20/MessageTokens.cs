namespace Anroo.OrviboS20
{
    internal class MessageTokens
    {
        public static readonly byte[] CommandPrefix = {0x68, 0x64};
        public static readonly byte[] MacAddressPadding = {0x20, 0x20, 0x20, 0x20, 0x20, 0x20};
        public static readonly byte[] EmptyPadding = {0x0, 0x0, 0x0, 0x0};

        public static readonly byte[] DiscoverUnknownRequestLength = {0x0, 0x6};
        public static readonly byte[] DiscoverUnknownRequestCode = {0x71, 0x61};
        public static readonly byte[] DiscoverUnknownResponseCode = {0x71, 0x61};

        public static readonly byte[] DiscoverKnownRequestLength = {0x0, 0x12};
        public static readonly byte[] DiscoverKnownRequestCode = {0x71, 0x67};
        public static readonly int DiscoverKnownResponseLength = 42;
        public static readonly byte[] DiscoverKnownResponseCode = {0x71, 0x67};

        public static readonly byte[] SubscribeRequestLength = { 0x00, 0x1e };
        public static readonly byte[] SubscribeCode = { 0x63, 0x6C };
        public static readonly int SubscribeResponseLength = 24;

        public static readonly byte[] PowerSwitchRequestLength = {0x00, 0x17};
        public static readonly byte[] PowerSwitchRequestCode = {0x64, 0x63};
        public static readonly int PowerSwitchResponseLength = 23;
        public static readonly byte[] PowerSwitchResponseCode = {0x73, 0x66};
    }
}