using System;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Extensions
{
    public static class IConsoleExtensions
    {
        public static void WriteSuccess(this IConsole console, string text) =>
            console.WriteWithColor(text, ConsoleColor.Green);

        public static void WriteError(this IConsole console, string text) =>
            console.WriteWithColor(text, ConsoleColor.Red);

        public static void WriteWarning(this IConsole console, string text) =>
            console.WriteWithColor(text, ConsoleColor.Yellow);

        private static void WriteWithColor(this IConsole console, string text, ConsoleColor color)
        {
            console.ForegroundColor = color;
            console.Error.WriteLine(text);
            console.ResetColor();
        }
    }
}