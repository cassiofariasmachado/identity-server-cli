using System;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Extensions
{
    public static class IConsoleExtensions
    {
        public static void WriteSuccess(this IConsole console, string text) =>
            console.WriteOutWithColor(text, ConsoleColor.Green);

        public static void WriteWarning(this IConsole console, string text) =>
            console.WriteOutWithColor(text, ConsoleColor.Yellow);

        public static void WriteError(this IConsole console, string text) =>
            console.WriteErrorWithColor(text, ConsoleColor.Red);

        private static void WriteOutWithColor(this IConsole console, string text, ConsoleColor color)
        {
            console.ForegroundColor = color;
            console.Out.WriteLine(text);
            console.ResetColor();
        }

        private static void WriteErrorWithColor(this IConsole console, string text, ConsoleColor color)
        {
            console.ForegroundColor = color;
            console.Error.WriteLine(text);
            console.ResetColor();
        }
    }
}