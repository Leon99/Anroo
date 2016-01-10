using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Anroo.Common;
using Anroo.Common.Udp.Cli;
using Anroo.OrviboS20.Cli.Properties;

namespace Anroo.OrviboS20.Cli
{
    class Program : ProgramBase<CommandLineArgs.CommandName>
    {
        private Program(Settings settings) : base(settings)
        {
        }

        static void Main(string[] args)
        {
            var app = new Program(Settings.Default);
            app.ExecuteProgram(() => new CommandLineArgs(args));
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
                OrviboS20PowerStateResponse result;
                switch (commandName)
                {
                    case CommandLineArgs.CommandName.On:
                        result = controller.PowerOnAsync().Result;
                        break;
                    case CommandLineArgs.CommandName.Off:
                        result = controller.PowerOffAsync().Result;
                        break;
                }
            }
        }

        protected override IEnumerable<HostIdentity> DiscoverHostIdentities(IPAddress localIP)
        {
            return OrviboS20Manager.DiscoverUnknownAsync(localIP).Result.Select(response => response.NetworkAddress);
        }
    }
}
