using System.Collections.Generic;
using IdentityServerCli.Console.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Test.Utils
{
    public static class CommandLineApplicationUtils
    {
        public static CommandLineApplication CreateCommandLineApplication(
            string commandName, string subCommandName, ICommand command
        )
        {
            var app = new CommandLineApplication();

            app.Command(commandName, newCmd =>
            {
                newCmd.Command(subCommandName, command.Execute);
            });

            return app;
        }

        public static IEnumerable<string> CreateMultipleOptionArguments(string optionName, string[] claims)
        {
            foreach (var claim in claims)
            {
                yield return optionName;
                yield return claim;
            }
        }
    }
}