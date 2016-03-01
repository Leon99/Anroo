using System;

namespace Anroo.Common
{
    class CommonHelpers
    {
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                Environment.Exit(-1);
            }
        }
    }
}
