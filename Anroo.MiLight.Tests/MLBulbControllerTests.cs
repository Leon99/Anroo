using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Anroo.Common;
using Should;
using Xunit;

namespace Anroo.MiLight.Tests
{
    [Trait("Category", "IgnoreForCI")]
    public class MLBulbControllerTests
    {
        private static readonly TimeSpan DelayBetweenCommands = TimeSpan.FromSeconds(2);

        [Fact]
        public void DualWhite_FullTest()
        {
            var bridgeAddresses = DiscoverBridges();

            foreach (var bridgeAddress in bridgeAddresses)
            {
                var bulbController = new MLDWBulbController(bridgeAddress.IPAddress);
                for (int groupIdx = 0; groupIdx < 5; groupIdx++)
                {
                    //groupIdx = 3;
                    MLBulbGroupCode group = (MLBulbGroupCode) groupIdx;

                    bulbController.OnAsync(group).Wait();

                    Pause();

                    for (int i = 0; i < 10; i++)
                    {
                        bulbController.BrightnessDownAsync(group).Wait();
                    }

                    Pause();

                    for (int i = 0; i < 10; i++)
                    {
                        bulbController.BrightnessUpAsync(group).Wait();
                    }

                    Pause();

                    for (int i = 0; i < 10; i++)
                    {
                        bulbController.ColorTemperatureDownAsync(group).Wait();
                    }

                    Pause();

                    for (int i = 0; i < 10; i++)
                    {
                        bulbController.ColorTemperatureUpAsync(group).Wait();
                    }

                    Pause();

                    bulbController.NightModeAsync(group).Wait();

                    Pause();

                    bulbController.FullBrightnessAsync(group).Wait();

                    Pause();

                    bulbController.OffAsync(group).Wait();

                    Pause();
                }
            }
        }

        [Fact]
        public void Rgbw_FullTest()
        {
            var bridgeAddresses = DiscoverBridges();

            foreach (var bridgeAddress in bridgeAddresses)
            {
                var bulbController = new MLRgbwBulbController(bridgeAddress.IPAddress);
                for (int groupIdx = 0; groupIdx < 5; groupIdx++)
                {
                    //groupIdx = 2;
                    MLBulbGroupCode group = (MLBulbGroupCode)groupIdx;

                    bulbController.OnAsync(group).Wait();

                    Pause();

                    bulbController.WhiteModeAsync(group).Wait();

                    Pause();

                    for (int i = 27; i > 1; i--)
                    {
                        bulbController.BrightnessAsync((byte) i).Wait();
                        Task.Delay(200).Wait();
                    }

                    Pause();

                    for (int i = 0; i < 256; i++)
                    {
                        bulbController.ColorAsync((byte)i).Wait();
                        Task.Delay(100).Wait();
                    }

                    Pause();

                    bulbController.NightModeAsync(group).Wait();

                    Pause();

                    bulbController.DiscoModeAsync(group).Wait();

                    Pause();

                    for (int i = 0; i < 10; i++)
                        bulbController.DiscoSpeedFasterAsync(group).Wait();

                    Pause();
                    Pause();

                    for (int i = 0; i < 10; i++)
                        bulbController.DiscoSpeedSlowerAsync(group).Wait();

                    Pause();

                    bulbController.OffAsync(group).Wait();

                    Pause();
                }
            }
        }

        [Fact]
        public void Rgbw_SingleGroupSequenceTest()
        {
            for (int i = 0; i < 50; i++)
            {
                using (var bulbController = new MLRgbwBulbController(IPAddress.Parse("192.168.1.42")))
                {
                    bulbController.OnAsync(MLBulbGroupCode.Two).Wait();
                }
                Pause();
            }
        }

        public static IEnumerable<HostIdentity> DiscoverBridges()
        {
            var discoveredAddresses = MLBridgeManager.DiscoverAsync(NetworkTools.GetLocalIPAddress("Wi-Fi")).Result;

            discoveredAddresses.Any()
                .ShouldBeTrue();

            return discoveredAddresses;
        }

        private static void Pause()
        {
            Task.Delay(DelayBetweenCommands).Wait();
        }
    }
}