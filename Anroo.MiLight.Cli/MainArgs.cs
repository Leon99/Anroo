using System.Collections.Generic;
using DocoptNet;

namespace Anroo.MiLight.Cli
{
    internal class MainArgs
    {
        private const string DiscoverOptionKey = "--discover";
        private const string CommandArgumentKey = "<command>";
        private const string GroupArgumentKey = "<group>";
        private const string ParamArgumentKey = "<param>";

        internal enum BulbCommandName
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

        private readonly string _usageText = $@"Control MiLight-compatible bulbs

Usage:
  MiLight -?|--help 
  MiLight {DiscoverOptionKey}
  MiLight {CommandArgumentKey} [{GroupArgumentKey}] [{ParamArgumentKey}] [--bridge=<bridgeIP>]
  MiLight --bridge=<bridgeIP>

  Supported commands
    Dual white bulbs:
      {BulbCommandName.DWOn}
      {BulbCommandName.DWOff}
      {BulbCommandName.DWNight}
      {BulbCommandName.BrightnessFull}
      {BulbCommandName.BrightnessUp}
      {BulbCommandName.BrightnessDown}
      {BulbCommandName.Warmer}
      {BulbCommandName.Cooler}
    RGBW bulbs:
      {BulbCommandName.RGBWOn}
      {BulbCommandName.RGBWOff}
      {BulbCommandName.RGBWNight}
      {BulbCommandName.White}
      {BulbCommandName.Brightness} <brightness> (2-27)
      {BulbCommandName.Color} <color> (0-255)
      {BulbCommandName.Disco}
      {BulbCommandName.DiscoFaster}
      {BulbCommandName.DiscoSlower}

Options:
  -? --help               Show this screen.
  --discover              Trigger autodiscovery and show IP addresses of available bridges.
  --bridge=<bridgeIP>     IP address of the bridge to use.
                          Will be stored as default when used without the command.";

        private readonly IDictionary<string, ValueObject> _args;
        public MainArgs(ICollection<string> argv, bool help = true,
                                                      object version = null, bool optionsFirst = false, bool exit = false)
        {
            _args = new Docopt().Apply(_usageText, argv, help, version, optionsFirst, exit);
        }

        public IDictionary<string, ValueObject> Args => _args;

        public bool DiscoverOptionSpecified => _args[DiscoverOptionKey].IsTrue;
        public string BridgeOptionValue => _args["--bridge"]?.ToString();
        public string CommandArgumentValue => _args[CommandArgumentKey]?.ToString();
        public string GroupArgumentValue => _args[GroupArgumentKey]?.ToString();
        public string ParamArgumentValue => _args[ParamArgumentKey]?.ToString();
        public bool HelpOptionSpecified => _args["--help"].IsTrue;
    }
}

