using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using DocoptNet;

namespace Anroo.Common.Udp.Cli
{
    public abstract class ProgramBase<TCommand> where TCommand : struct
    {
        private readonly ApplicationSettingsBase _settings;

        protected ProgramBase(ApplicationSettingsBase settings)
        {
            _settings = settings;
        }

        protected abstract void RunCommand(CommandLineArgsBase parsedArgs);
        protected abstract IEnumerable<HostIdentity> DiscoverHostIdentities(IPAddress localIP);

        protected void ExecuteProgram(Func<CommandLineArgsBase> argsParserFactory)
        {
            try
            {
                _settings.Upgrade();
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

                if (parsedArgs.DiscoverOptionSpecified)
                {
                    RunDiscover();
                }
                else if (!string.IsNullOrEmpty(parsedArgs.CommandArgumentValue))
                {
                    RunCommand(parsedArgs);
                }
                else
                {
                    if (!string.IsNullOrEmpty(parsedArgs.IPOptionValue))
                    {
                        if (!StoreIP(parsedArgs.IPOptionValue, _settings))
                        {
                            ConsoleHelpers.WriteError("Unable to store specified IP address.");
                        }
                    }
                    if (!string.IsNullOrEmpty(parsedArgs.MacOptionValue))
                    {
                        if (!StoreMac(parsedArgs.MacOptionValue, _settings))
                        {
                            ConsoleHelpers.WriteError("Unable to store specified MAC address.");
                        }
                    }
                }
            }
            finally
            {
                ConsoleHelpers.WaitForEnter();
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

        private void RunDiscover()
        {
            var localEPs = NetworkTools.GetEligibleLocalEndpoints();
            foreach (var localEP in localEPs)
            {
                Console.WriteLine();
                Console.Write($"Looking for things on '{localEP.NetworkInterfaceName}'... ");
                var things = DiscoverHostIdentities(localEP.UnicastAddress);
                if (things.Any())
                {
                    Console.WriteLine($"Found {things.Count()} thing(s):");
                    Console.WriteLine();
                    foreach (var thing in things)
                    {
                        Console.WriteLine($"\tIP: {thing.IPAddress}\tMAC: {thing.MacAddress.ToMacAddressString()}");
                    }
                }
                else
                {
                    Console.WriteLine("Not found.");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Discover finished.");
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
                : _settings["ThingIP"] as string;
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
                : _settings["ThingMac"] as string;
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