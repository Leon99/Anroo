using System;
using System.Collections.Generic;
using System.IO;
using DocoptNet;

namespace Anroo.Common.Cli
{
    public class CommandLineArgsBase
    {
        protected static readonly string AppName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
        protected virtual string UsageText { get; }
        protected IDictionary<string, ValueObject> _args;

        public CommandLineArgsBase(ICollection<string> argv, bool help = true,
            object version = null, bool optionsFirst = false, bool exit = false)
        {
            _args = new Docopt().Apply(UsageText, argv, help, version, optionsFirst, exit);
        }

        protected const string DiscoverOptionKey = "--discover";
        protected const string IPOptionKey = "--ip";
        protected const string MacOptionKey = "--mac";
        protected const string CommandArgumentKey = "<command>";
        public IDictionary<string, ValueObject> Args => _args;
        public bool HelpOptionSpecified => _args["--help"].IsTrue;
        public bool DiscoverOptionSpecified => _args[DiscoverOptionKey].IsTrue;
        public string IPOptionValue => _args.ContainsKey(IPOptionKey) ? _args[IPOptionKey]?.ToString() : null;
        public string MacOptionValue => _args.ContainsKey(MacOptionKey) ? _args[MacOptionKey]?.ToString() : null;
        public string CommandArgumentValue => _args[CommandArgumentKey]?.ToString();
    }
}