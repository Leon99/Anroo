using System;
using System.Diagnostics;

namespace Anroo.Common.Cli
{
    public static class ConsoleHelpers
    {
        public static void WriteError(string text)
        {
            using (new ConsoleColorCookie(ConsoleColor.Red))
            {
                Console.Error.WriteLine($"ERROR: {text}");
            }
        }

        [Conditional("DEBUG")]
        public static void WaitForEnter()
        {
            Console.WriteLine();
            Console.WriteLine("Press <Enter> to continue.");
            Console.ReadLine();
        }
    }
}