using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DocoptNet;

namespace Anroo.Common.Cli
{
    public class CommandLineArgsBase
    {
        protected static readonly string AppName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().GetName().Name);

        private const string HelpOptionKey = "--help";
        protected const string DiscoverOptionKey = "--discover";
        protected const string IPOptionKey = "--ip";
        protected const string MacOptionKey = "--mac";
        protected const string CommandArgumentKey = "<command>";

        public virtual string UsageText { get; }

        public IDictionary<string, ValueObject> Args { get; }
        public bool HelpOptionSpecified => Args[HelpOptionKey].IsTrue;
        public bool DiscoverOptionSpecified => Args[DiscoverOptionKey].IsTrue;
        public string IPOptionValue => Args.ContainsKey(IPOptionKey) ? Args[IPOptionKey]?.ToString() : null;
        public string MacOptionValue => Args.ContainsKey(MacOptionKey) ? Args[MacOptionKey]?.ToString() : null;
        public string CommandArgumentValue => Args[CommandArgumentKey]?.ToString();

        public CommandLineArgsBase(ICollection<string> argv, bool help = true,
            object version = null, bool optionsFirst = false, bool exit = false)
        {
            Args = new Docopt().Apply(UsageText, argv, help, version, optionsFirst, exit);
        }

    }
}