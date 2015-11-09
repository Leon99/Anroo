namespace Anroo.MiLight
{
    internal static class MLBulbCommands
    {
        public static class DualWhite
        {
            public static readonly byte[] AllOn = { 0x35, 0x0, 0x55 };
            public static readonly byte[] AllOff = { 0x39, 0x0, 0x55 };
            public static readonly byte[] Group1On = { 0x38, 0x0, 0x55 };
            public static readonly byte[] Group1Off = { 0x3B, 0x0, 0x55 };
            public static readonly byte[] Group2On = { 0x3D, 0x0, 0x55 };
            public static readonly byte[] Group2Off = { 0x33, 0x0, 0x55 };
            public static readonly byte[] Group3On = { 0x37, 0x0, 0x55 };
            public static readonly byte[] Group3Off = { 0x3A, 0x0, 0x55 };
            public static readonly byte[] Group4On = { 0x32, 0x0, 0x55 };
            public static readonly byte[] Group4Off = { 0x36, 0x0, 0x55 };

            public static readonly byte[] AllFullBrightness = { 0xB5, 0x0, 0x55 };
            public static readonly byte[] Group1FullBrightness = { 0xB8, 0x0, 0x55 };
            public static readonly byte[] Group2FullBrightness = { 0xBD, 0x0, 0x55 };
            public static readonly byte[] Group3FullBrightness = { 0xB7, 0x0, 0x55 };
            public static readonly byte[] Group4FullBrightness = { 0xB2, 0x0, 0x55 };

            public static readonly byte[] AllNightMode = { 0xB9, 0x0, 0x55 };
            public static readonly byte[] Group1NightMode = { 0xBB, 0x0, 0x55 };
            public static readonly byte[] Group2NightMode = { 0xB3, 0x0, 0x55 };
            public static readonly byte[] Group3NightMode = { 0xBA, 0x0, 0x55 };
            public static readonly byte[] Group4NightMode = { 0xB6, 0x0, 0x55 };

            public static readonly byte[] BrightnessUp = { 0x3C, 0x0, 0x55 };
            public static readonly byte[] BrightnessDown = { 0x34, 0x0, 0x55 };

            public static readonly byte[] ColorTemperatureUp = { 0x3F, 0x0, 0x55 };
            public static readonly byte[] ColorTemperatureDown = { 0x3E, 0x0, 0x55 };
        }

        public static class Rgb
        {
            public static readonly byte[] On = { 0x22, 0x0, 0x55 };
            public static readonly byte[] Off = { 0x21, 0x0, 0x55 };
            public static readonly byte[] BrightnessUp = { 0x23, 0x0, 0x55 };
            public static readonly byte[] BrightnessDown = { 0x24, 0x0, 0x55 };
            public static readonly byte[] SpeedUp = { 0x25, 0x0, 0x55 };
            public static readonly byte[] SpeedDown = { 0x26, 0x0, 0x55 };
            public static readonly byte[] DiscoNext = { 0x27, 0x0, 0x55 };
            public static readonly byte[] DiscoLast = { 0x28, 0x0, 0x55 };
            public static readonly byte[] Color = { 0x20, 0x0, 0x55 };
        }

        public static class Rgbw
        {
            public static readonly byte[] AllOn = { 0x42, 0x0 };
            public static readonly byte[] AllOff = { 0x41, 0x0 };
            public static readonly byte[] AllNight = { 0xC1, 0x0 };
            public static readonly byte[] Group1On = { 0x45, 0x0 };
            public static readonly byte[] Group1Off = { 0x46, 0x0 };
            public static readonly byte[] Group1Night = { 0xC6, 0x0 };
            public static readonly byte[] Group2On = { 0x47, 0x0 };
            public static readonly byte[] Group2Off = { 0x48, 0x0 };
            public static readonly byte[] Group2Night = { 0xC8, 0x0 };
            public static readonly byte[] Group3On = { 0x49, 0x0 };
            public static readonly byte[] Group3Off = { 0x4A, 0x0 };
            public static readonly byte[] Group3Night = { 0xCA, 0x0 };
            public static readonly byte[] Group4On = { 0x4B, 0x0 };
            public static readonly byte[] Group4Off = { 0x4C, 0x0 };
            public static readonly byte[] Group4Night = { 0xCC, 0x0 };

            public static readonly byte[] AllWhite = { 0xC2, 0x0 };
            public static readonly byte[] Group1White = { 0xC5, 0x0 };
            public static readonly byte[] Group2White = { 0xC7, 0x0 };
            public static readonly byte[] Group3White = { 0xC9, 0x0 };
            public static readonly byte[] Group4White = { 0xCB, 0x0 };

            public static readonly byte[] Brightness = { 0x4E, 0x0 };
            public static readonly byte[] Color = { 0x40, 0x0 };

            public static readonly byte[] DiscoMode = { 0x4D, 0x0 };
            public static readonly byte[] DiscoSpeedSlower = { 0x43, 0x0 };
            public static readonly byte[] DiscoSpeedFaster = { 0x44, 0x0 };
        }
    }
}