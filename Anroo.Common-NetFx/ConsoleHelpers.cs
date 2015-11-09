using System;
using System.Diagnostics;
using Anroo.MiLight.Cli;

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