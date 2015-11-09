using System;
using System.Linq;
using System.Net;
using Anroo.Common;
using Anroo.MiLight.Cli.Properties;

namespace Anroo.MiLight.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parsedArgs = new MainArgs(args, exit: true);
            if (parsedArgs.DiscoverOptionSpecified)
            {
                RunDiscover();
            }
            else if (!string.IsNullOrEmpty(parsedArgs.CommandArgumentValue))
            {
                RunBulbCommand(parsedArgs);
            }
            else if (!string.IsNullOrEmpty(parsedArgs.BridgeOptionValue))
            {
                StoreBridge(parsedArgs);
            }
            ConsoleHelpers.WaitForEnter();
        }

        private static void StoreBridge(MainArgs parsedArgs)
        {
            IPAddress bridgeIp;
            if (!TryParseIP(parsedArgs.BridgeOptionValue, out bridgeIp))
            {
                return;
            }
            Settings.Default.BridgeIP = parsedArgs.BridgeOptionValue;
            Settings.Default.Save();
            Console.WriteLine($"'{parsedArgs.BridgeOptionValue}' stored as default bridge IP.");
        }

        private static void RunBulbCommand(MainArgs parsedArgs)
        {
            // Verify command
            MainArgs.BulbCommandName commandName;
            if (!Enum.TryParse(parsedArgs.CommandArgumentValue, true, out commandName))
            {
                ConsoleHelpers.WriteError($"Specified command ('{parsedArgs.CommandArgumentValue}') is invalid.");
                return;
            }

            // Verify group number
            byte group = (byte)MLBulbGroupCode.All;
            if (parsedArgs.GroupArgumentValue != null)
            {
                if (!byte.TryParse(parsedArgs.GroupArgumentValue, out group) || group > 4)
                {
                    ConsoleHelpers.WriteError($"Specified group number ('{parsedArgs.GroupArgumentValue}') is invalid.");
                    return;
                }
            }
            var groupCode = (MLBulbGroupCode)group;

            // Verify bridge IP
            var bridgeIPStr = !string.IsNullOrEmpty(parsedArgs.BridgeOptionValue) ? parsedArgs.BridgeOptionValue : Settings.Default.BridgeIP;
            if (string.IsNullOrEmpty(bridgeIPStr))
            {
                ConsoleHelpers.WriteError("Please specify IP address of the bridge.");
                return;
            }
            IPAddress bridgeIP;
            if (!TryParseIP(bridgeIPStr, out bridgeIP))
            {
                return;
            }

            switch (commandName)
            {
                case MainArgs.BulbCommandName.DWOn:
                case MainArgs.BulbCommandName.DWOff:
                case MainArgs.BulbCommandName.DWNight:
                case MainArgs.BulbCommandName.BrightnessFull:
                case MainArgs.BulbCommandName.BrightnessUp:
                case MainArgs.BulbCommandName.BrightnessDown:
                case MainArgs.BulbCommandName.Warmer:
                case MainArgs.BulbCommandName.Cooler:
                {
                    var bulbGroup = new MLDWBulbController(bridgeIP, groupCode);
                    switch (commandName)
                    {
                        case MainArgs.BulbCommandName.DWOn:
                            bulbGroup.OnAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.DWOff:
                            bulbGroup.OffAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.DWNight:
                            bulbGroup.NightModeAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.BrightnessFull:
                            bulbGroup.FullBrightnessAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.BrightnessUp:
                            bulbGroup.BrightnessUpAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.BrightnessDown:
                            bulbGroup.BrightnessDownAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.Warmer:
                            bulbGroup.ColorTemperatureDownAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.Cooler:
                            bulbGroup.ColorTemperatureUpAsync().Wait();
                            break;
                    }
                }
                    break;
                case MainArgs.BulbCommandName.RGBWOn:
                case MainArgs.BulbCommandName.RGBWOff:
                case MainArgs.BulbCommandName.RGBWNight:
                case MainArgs.BulbCommandName.Brightness:
                case MainArgs.BulbCommandName.White:
                case MainArgs.BulbCommandName.Color:
                case MainArgs.BulbCommandName.Disco:
                case MainArgs.BulbCommandName.DiscoFaster:
                case MainArgs.BulbCommandName.DiscoSlower:
                {
                    var bulbGroup = new MLRgbwBulbController(bridgeIP, groupCode);
                    switch (commandName)
                    {
                        case MainArgs.BulbCommandName.RGBWOn:
                            bulbGroup.OnAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.RGBWOff:
                            bulbGroup.OffAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.RGBWNight:
                            bulbGroup.NightModeAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.Brightness:
                            byte brightness;
                            if (!byte.TryParse(parsedArgs.ParamArgumentValue, out brightness)
                                || !brightness.Between(MLDWBulbController.MinBrightness, MLDWBulbController.MaxBrightness))
                            {
                                ConsoleHelpers.WriteError("Specified brightness value is invalid.");
                                return;
                            }
                            bulbGroup.BrightnessAsync(brightness).Wait();
                            break;
                            case MainArgs.BulbCommandName.White:
                                bulbGroup.WhiteModeAsync().Wait();
                                break;
                            case MainArgs.BulbCommandName.Color:
                            byte color;
                            if (!byte.TryParse(parsedArgs.ParamArgumentValue, out color))
                            {
                                ConsoleHelpers.WriteError("Specified color is invalid.");
                                return;
                            }
                            bulbGroup.ColorAsync(color).Wait();
                            break;
                        case MainArgs.BulbCommandName.Disco:
                            bulbGroup.DiscoModeAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.DiscoFaster:
                            bulbGroup.DiscoSpeedFasterAsync().Wait();
                            break;
                        case MainArgs.BulbCommandName.DiscoSlower:
                            bulbGroup.DiscoSpeedSlowerAsync().Wait();
                            break;
                    }
                }
                    break;
            }
            Console.WriteLine($"'{parsedArgs.CommandArgumentValue}' command has been sent.");
        }

        private static bool TryParseIP(string s, out IPAddress ip)
        {
            if (!IPAddress.TryParse(s, out ip))
            {
                ConsoleHelpers.WriteError("Format of the specified IP address is invalid.");
                return false;
            }
            return true;
        }

        private static void RunDiscover()
        {
            var localEPs = NetworkTools.GetEligibleLocalEndpoints();
            foreach (var localEP in localEPs)
            {
                Console.WriteLine();
                Console.Write($"Looking for bridges on '{localEP.NetworkInterfaceName}'... ");
                var bridges = MLBridgeManager.DiscoverBridgesAsync(localEP.UnicastAddress).Result;
                if (bridges.Any())
                {
                    Console.WriteLine($"Found {bridges.Count()} bridge(s):");
                    Console.WriteLine();
                    foreach (var bridge in bridges)
                    {
                        Console.WriteLine($"\t{bridge.IPAddress} (MAC: {bridge.MacAddress.ToMacAddressString()})");
                    }
                }
                else
                {
                    Console.WriteLine("No bridges found.");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Discover finished.");
        }
    }
}