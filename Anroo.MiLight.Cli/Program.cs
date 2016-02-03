using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Anroo.Common;
using Anroo.Common.Udp.Cli;
using Anroo.MiLight.Cli.Properties;

namespace Anroo.MiLight.Cli
{
    internal class Program: ProgramBase<CommandLineArgs.CommandName>
    {
        public Program(ApplicationSettingsBase settings) : base(settings)
        {
        }

        private static void Main(string[] args)
        {
            var app = new Program(Settings.Default);
            app.ExecuteProgram(() => new CommandLineArgs(args));
        }


        protected override void RunCommand(CommandLineArgsBase baseParsedArgs)
        {
            CommandLineArgs parsedArgs = (CommandLineArgs)baseParsedArgs;
            CommandLineArgs.CommandName commandName;
            MLBulbGroupCode groupCode;
            IPAddress thingIP;

            if (!VerifyIP(baseParsedArgs, out thingIP)) return;
            if (!VerifyCommand(baseParsedArgs, out commandName)) return;
            if (!VerifyGroup(parsedArgs, out groupCode)) return;

            switch (commandName)
            {
                case CommandLineArgs.CommandName.DWOn:
                case CommandLineArgs.CommandName.DWOff:
                case CommandLineArgs.CommandName.DWNight:
                case CommandLineArgs.CommandName.BrightnessFull:
                case CommandLineArgs.CommandName.BrightnessUp:
                case CommandLineArgs.CommandName.BrightnessDown:
                case CommandLineArgs.CommandName.Warmer:
                case CommandLineArgs.CommandName.Cooler:
                {
                    using (var bulbGroup = new MLDWBulbController(thingIP, groupCode))
                    {
                        switch (commandName)
                        {
                            case CommandLineArgs.CommandName.DWOn:
                                bulbGroup.OnAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.DWOff:
                                bulbGroup.OffAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.DWNight:
                                bulbGroup.NightModeAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.BrightnessFull:
                                bulbGroup.FullBrightnessAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.BrightnessUp:
                                bulbGroup.BrightnessUpAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.BrightnessDown:
                                bulbGroup.BrightnessDownAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.Warmer:
                                bulbGroup.ColorTemperatureDownAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.Cooler:
                                bulbGroup.ColorTemperatureUpAsync().Wait();
                                break;
                        }
                    }
                }
                    break;
                case CommandLineArgs.CommandName.RGBWOn:
                case CommandLineArgs.CommandName.RGBWOff:
                case CommandLineArgs.CommandName.RGBWNight:
                case CommandLineArgs.CommandName.Brightness:
                case CommandLineArgs.CommandName.White:
                case CommandLineArgs.CommandName.Color:
                case CommandLineArgs.CommandName.Disco:
                case CommandLineArgs.CommandName.DiscoFaster:
                case CommandLineArgs.CommandName.DiscoSlower:
                {
                    using (var bulbGroup = new MLRgbwBulbController(thingIP, groupCode))
                    {
                        switch (commandName)
                        {
                            case CommandLineArgs.CommandName.RGBWOn:
                                bulbGroup.OnAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.RGBWOff:
                                bulbGroup.OffAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.RGBWNight:
                                bulbGroup.NightModeAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.Brightness:
                                byte brightness;
                                if (!byte.TryParse(parsedArgs.CommandParamArgumentValue, out brightness)
                                    || !brightness.Between(MLDWBulbController.MinBrightness, MLDWBulbController.MaxBrightness))
                                {
                                    ConsoleHelpers.WriteError("Specified brightness value is invalid.");
                                    return;
                                }
                                bulbGroup.BrightnessAsync(brightness).Wait();
                                break;
                            case CommandLineArgs.CommandName.White:
                                bulbGroup.WhiteModeAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.Color:
                                byte color;
                                if (!byte.TryParse(parsedArgs.CommandParamArgumentValue, out color))
                                {
                                    ConsoleHelpers.WriteError("Specified color is invalid.");
                                    return;
                                }
                                bulbGroup.ColorAsync(color).Wait();
                                break;
                            case CommandLineArgs.CommandName.Disco:
                                bulbGroup.DiscoModeAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.DiscoFaster:
                                bulbGroup.DiscoSpeedFasterAsync().Wait();
                                break;
                            case CommandLineArgs.CommandName.DiscoSlower:
                                bulbGroup.DiscoSpeedSlowerAsync().Wait();
                                break;
                        }
                    }
                }
                    break;
            }
            Console.WriteLine($"'{baseParsedArgs.CommandArgumentValue}' command has been sent to group '{groupCode}' via {thingIP}.");
        }

        private bool VerifyGroup(CommandLineArgs parsedArgs, out MLBulbGroupCode groupCode)
        {
            // Verify group number
            byte group = (byte)MLBulbGroupCode.All;
            if (parsedArgs.GroupArgumentValue != null)
            {
                if (!byte.TryParse(parsedArgs.GroupArgumentValue, out @group) || @group > 4)
                {
                    ConsoleHelpers.WriteError($"Specified group number ('{parsedArgs.GroupArgumentValue}') is invalid.");
                    groupCode = MLBulbGroupCode.All;
                    return false;
                }
            }
            groupCode = (MLBulbGroupCode)@group;
            return true;
        }

        protected override IEnumerable<HostIdentity> DiscoverHostIdentities(IPAddress localIP)
        {
            return MLBridgeManager.DiscoverAsync(localIP).Result;
        }
    }
}