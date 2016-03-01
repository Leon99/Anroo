using System.Collections.Generic;
using Anroo.Common.Cli;

namespace Anroo.MiLight.Cli
{
    internal class CommandLineArgs : CommandLineArgsBase
    {
        internal enum CommandName
        {
            DWOn,
            DWOff,
            DWNight,
            BrightnessFull,
            BrightnessUp,
            BrightnessDown,
            Warmer,
            Cooler,
            RGBWOn,
            RGBWOff,
            RGBWNight,
            White,
            Brightness,
            Color,
            Disco,
            DiscoFaster,
            DiscoSlower,
        }

        private const string GroupArgumentKey = "<group>";
        private const string CommandParamArgumentKey = "<param>";
        protected const string ProtocolOptionKey = "--proto";

        protected override string UsageText => 
$@"Control MiLight-compatible bulbs

Usage:
  {AppName} -?|--help 
  {AppName} {DiscoverOptionKey}
  {AppName} [{GroupArgumentKey}] {CommandArgumentKey} [{CommandParamArgumentKey}] [{IPOptionKey}=<bridgeIP>] [{ProtocolOptionKey}=<protocol>]
  {AppName} {CommandArgumentKey} [{CommandParamArgumentKey}] [{IPOptionKey}=<bridgeIP>] [{ProtocolOptionKey}=<protocol>]
  {AppName} [{IPOptionKey}=<bridgeIP>] [{ProtocolOptionKey}=<protocol>]

  Default {GroupArgumentKey} is 0 (all groups).
  
Supported commands
  Dual white bulbs:
    {CommandName.DWOn}
    {CommandName.DWOff}
    {CommandName.DWNight}
    {CommandName.BrightnessFull}
    {CommandName.BrightnessUp}
    {CommandName.BrightnessDown}
    {CommandName.Warmer}
    {CommandName.Cooler}
  RGBW bulbs:
    {CommandName.RGBWOn}
    {CommandName.RGBWOff}
    {CommandName.RGBWNight}
    {CommandName.White}
    {CommandName.Brightness}
        <param>: brightness, possible values: 2-27
    {CommandName.Color}
        <param>: color code, possible values: 0-255
    {CommandName.Disco}
    {CommandName.DiscoFaster}
    {CommandName.DiscoSlower}

Options:
  -? --help          Show this screen.
  
  --discover         Show available bridges.

  --ip=<bridgeIP>    IP address of the bridge to use.
                     Will be stored as default when used without the command.

  --proto=<protocol> Protocol used to send commands to the bridge. Supported values:
                        UDP (default)
                        TCP";

        public CommandLineArgs(ICollection<string> argv, bool help = true, object version = null, bool optionsFirst = false, bool exit = false)
            : base(argv, help, version, optionsFirst, exit)
        {
        }
        public string GroupArgumentValue => _args[GroupArgumentKey]?.ToString();
        public string CommandParamArgumentValue => _args[CommandParamArgumentKey]?.ToString();
        public string ProtocolOptionValue => _args.ContainsKey(ProtocolOptionKey) ? _args[ProtocolOptionKey]?.ToString() : null;
    }
}