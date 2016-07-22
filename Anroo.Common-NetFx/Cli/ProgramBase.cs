 using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
 using Anroo.Common.Extensions;
 using Anroo.Common.Network;
using DocoptNet;

namespace Anroo.Common.Cli
{
    public abstract class ProgramBase<TCommand> where TCommand : struct
    {
        protected readonly ApplicationSettingsBase Settings;

        protected ProgramBase(ApplicationSettingsBase settings)
        {
            Settings = settings;
        }

        protected abstract void RunCommand(CommandLineArgsBase parsedArgs);
        protected abstract Task<IEnumerable<HostIdentity>> DiscoverHostIdentitiesAsync(IPAddress localIP);

        protected void ExecuteProgram(Func<CommandLineArgsBase> argsParserFactory)
        {
            if (Environment.HasShutdownStarted)
            {
                return;
            }
            try
            {
                bool isUpgradeRequired;
                try
                {
                    isUpgradeRequired = (bool) Settings["IsUpgradeRequired"];
                }
                catch (SettingsPropertyNotFoundException)
                {
                    isUpgradeRequired = true;
                }
                if (isUpgradeRequired)
                {
                    Settings.Upgrade();
                    Settings["IsUpgradeRequired"] = false;
                    Settings.Save();
                }
                CommandLineArgsBase parsedArgs;
                try
                {
                    parsedArgs = argsParserFactory();
                }
                catch (DocoptBaseException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                if (Environment.GetCommandLineArgs().Length == 1)
                {
                    Console.WriteLine(parsedArgs.UsageText);
                }

                if (parsedArgs.DiscoverOptionSpecified)
                {
                    RunDiscoveryAsync().Wait();
                }
                else if (!string.IsNullOrEmpty(parsedArgs.CommandArgumentValue))
                {
                    RunCommand(parsedArgs);
                }
                else
                {
                    HandleSettingsOptions(parsedArgs);
                }
            }
            finally
            {
                ConsoleHelpers.WaitForEnter();
            }
        }

        protected virtual void HandleSettingsOptions(CommandLineArgsBase baseParsedArgs)
        {
            if (!string.IsNullOrEmpty(baseParsedArgs.IPOptionValue))
            {
                if (!StoreIP(baseParsedArgs.IPOptionValue, Settings))
                {
                    ConsoleHelpers.WriteError("Unable to store specified IP address.");
                }
            }
            if (!string.IsNullOrEmpty(baseParsedArgs.MacOptionValue))
            {
                if (!StoreMac(baseParsedArgs.MacOptionValue, Settings))
                {
                    ConsoleHelpers.WriteError("Unable to store specified MAC address.");
                }
            }
        }

        private static bool StoreIP(string thingIPString, ApplicationSettingsBase settings)
        {
            IPAddress thingIP;
            if (!TryParseIP(thingIPString, out thingIP))
            {
                return false;
            }
            settings["ThingIP"] = thingIPString;
            settings.Save();
            Console.WriteLine($"'{thingIPString}' stored as default IP address.");
            return true;
        }

        protected static bool TryParseIP(string s, out IPAddress ip)
        {
            if (!IPAddress.TryParse(s, out ip))
            {
                ConsoleHelpers.WriteError("Format of the specified IP address is invalid.");
                return false;
            }
            return true;
        }
        private static bool StoreMac(string thingMacString, ApplicationSettingsBase settings)
        {
            PhysicalAddress thingMac;
            if (!TryParseMac(thingMacString, out thingMac))
            {
                return false;
            }
            settings["ThingMac"] = thingMacString;
            settings.Save();
            Console.WriteLine($"'{thingMacString}' stored as default MAC address.");
            return true;
        }

        protected static bool TryParseMac(string s, out PhysicalAddress mac)
        {
            if (!PhysicalAddressFactory.TryParse(s, out mac))
            {
                ConsoleHelpers.WriteError("Format of the specified MAC address is invalid.");
                return false;
            }
            return true;
        }

        private async Task RunDiscoveryAsync()
        {
            var localEPs = NetworkTools.GetEligibleLocalEndpoints();
            Console.WriteLine("Compatible interface(s) detected:");
            foreach (var localEP in localEPs)
            {
                Console.WriteLine($"- {localEP.NetworkInterfaceName}");
            }
            Console.WriteLine();
            Console.WriteLine("Looking for things...");
            Console.WriteLine();
            var tasks = localEPs.Select(localEP => DiscoverHostIdentitiesAsync(localEP.UnicastAddress)).ToArray();
            await Task.WhenAll(tasks);
            int discoveredThingsCnt = 0;
            foreach (var task in tasks)
            {
                var hosts = task.Result;
                foreach (var host in hosts)
                {
                    Console.WriteLine($"IP address: {host.IPAddress}\tMAC address: {host.MacAddress.ToMacAddressString()}");
                    discoveredThingsCnt++;
                }
            }
            if (discoveredThingsCnt == 0)
            {
                Console.WriteLine("No things found on any of the compatible interfaces.");
            }
            Console.WriteLine();
        }

        protected bool VerifyCommand(CommandLineArgsBase parsedArgs, out TCommand commandName)
        {
            if (!Enum.TryParse(parsedArgs.CommandArgumentValue, true, out commandName))
            {
                ConsoleHelpers.WriteError($"Specified command ('{parsedArgs.CommandArgumentValue}') is invalid.");
                return false;
            }
            return true;
        }

        protected bool VerifyIP(CommandLineArgsBase parsedArgs, out IPAddress thingIP)
        {
            thingIP = null;

            // Verify bridge IP
            var thingIPStr = !string.IsNullOrEmpty(parsedArgs.IPOptionValue)
                ? parsedArgs.IPOptionValue
                : Settings["ThingIP"] as string;
            if (string.IsNullOrEmpty(thingIPStr))
            {
                ConsoleHelpers.WriteError("Please specify IP address of the thing.");
                return false;
            }
            if (!TryParseIP(thingIPStr, out thingIP))
            {
                return false;
            }
            return true;
        }
        protected bool VerifyMac(CommandLineArgsBase parsedArgs, out PhysicalAddress thingMac)
        {
            thingMac = null;

            // Verify MAC
            var thingMacStr = !string.IsNullOrEmpty(parsedArgs.MacOptionValue)
                ? parsedArgs.MacOptionValue
                : Settings["ThingMac"] as string;
            if (string.IsNullOrEmpty(thingMacStr))
            {
                ConsoleHelpers.WriteError("Please specify MAC address of the thing.");
                return false;
            }
            if (!TryParseMac(thingMacStr, out thingMac))
            {
                return false;
            }
            return true;
        }
    }
}