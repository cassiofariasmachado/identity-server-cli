using System;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Extensions
{
    public static class IConsoleExtensions
    {
        public static void WriteSuccess(this IConsole console, string text)
        {
            console.ForegroundColor = ConsoleColor.Green;
            console.WriteLine(text);
            console.ResetColor();
        }

        public static void WriteError(this IConsole console, string text)
        {
            console.ForegroundColor = ConsoleColor.Red;
            console.Error.WriteLine(text);
            console.ResetColor();
        }
    }
}