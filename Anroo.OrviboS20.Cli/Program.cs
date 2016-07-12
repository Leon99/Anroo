using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Anroo.Common.Cli;
using Anroo.Common.Network;
using Anroo.OrviboS20.Cli.Properties;

namespace Anroo.OrviboS20.Cli
{
    class Program : ProgramBase<CommandLineArgs.CommandName>
    {
        static void Main(string[] args)
        {
            var app = new Program(Properties.Settings.Default);
            app.ExecuteProgram(() => new CommandLineArgs(args));
        }

        private Program(Settings settings) : base(settings)
        {
        }

        protected override void RunCommand(CommandLineArgsBase baseParsedArgs)
        {
            CommandLineArgs.CommandName commandName;
            PhysicalAddress thingMac;
            IPAddress thingIP;
            if (!VerifyIP(baseParsedArgs, out thingIP)) return;
            if (!VerifyMac(baseParsedArgs, out thingMac)) return;
            if (!VerifyCommand(baseParsedArgs, out commandName)) return;

            using (var controller = new OrviboS20Controller(thingIP, thingMac.GetAddressBytes()))
            {
                Console.Write($"Sending '{commandName}' command to {thingIP}...");
                OrviboS20PowerStateResponse result = null;
                switch (commandName)
                {
                    case CommandLineArgs.CommandName.On:
                        result = controller.PowerOnAsync().Result;
                        break;
                    case CommandLineArgs.CommandName.Off:
                        result = controller.PowerOffAsync().Result;
                        break;
                }
                Console.WriteLine($"Result: {(result != null ? (result.IsPowerOn ? "Power On" : "Power Off") : "Unknown")}");
            }
        }

        protected override IEnumerable<HostIdentity> DiscoverHostIdentities(IPAddress localIP)
        {
            return OrviboS20Manager.DiscoverUnknownAsync(localIP).Result.Select(response => response.NetworkAddress);
        }
    }
}
