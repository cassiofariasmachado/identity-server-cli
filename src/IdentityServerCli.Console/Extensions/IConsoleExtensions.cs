using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void WriteTable(this IConsole console, IEnumerable<string> headers, IEnumerable<IEnumerable<string>> rows)
        {
            var maxValue = rows.SelectMany(c => c)
                .Concat(headers)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Length)
                .Max() + 1;

            string format = "|" + string.Concat(Enumerable.Range(0, headers.Count())
                    .Select((c, index) => $"{{{index},-{maxValue}}}|"));

            string header = string.Format(format, headers.ToArray());

            console.WriteLine(new string('-', header.Length));
            console.WriteLine(header);
            console.WriteLine(new string('-', header.Length));

            foreach (var row in rows)
            {
                string formatedRow = string.Format(format, row.ToArray());
                console.WriteLine(formatedRow);
            }


            console.WriteLine(new string('-', header.Length));
        }

        public static void WriteTable<T>(
            this IConsole console,
            IEnumerable<string> headers,
            IEnumerable<T> rows,
            Func<T, IEnumerable<string>> mapRow
        ) => console.WriteTable(headers, rows.Select(mapRow));
    }
}