using System.Collections.Generic;
using Anroo.Common.Udp.Cli;

namespace Anroo.OrviboS20.Cli
{
    internal class CommandLineArgs : CommandLineArgsBase
    {
        internal enum CommandName
        {
            On,
            Off,
        }

        protected override string UsageText =>
$@"Control Orvibo S20 power socket.

Usage:
  {AppName} -?|--help 
  {AppName} {DiscoverOptionKey}
  {AppName} {CommandArgumentKey} [{MacOptionKey}=<MACAddress> {IPOptionKey}=<IPAddress>]
  {AppName} {MacOptionKey}=<MACAddress> {IPOptionKey}=<IPAddress>

Supported commands:
  {CommandName.On}
  {CommandName.Off}

Options:
  -? --help      Show this screen.
  
  --discover     Show available sockets.

  --mac=<MACAddress>    MAC address of the socket to use.
                        Will be stored as default when used without the command.

  --ip=<IPAddress>      IP address of the socket to use.
                        Will be stored as default when used without the command.

  Both IP and MAC addresses must be specified to operate the socket.";

        public CommandLineArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false) 
            : base(argv, help, version, optionsFirst, exit)
        {
        }
    }
}