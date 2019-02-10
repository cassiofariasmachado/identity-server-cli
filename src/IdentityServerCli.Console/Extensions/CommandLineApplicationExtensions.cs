using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Extensions
{
    public static class CommandLineApplicationExtensions
    {
        private const string DefaultTemplate = "--{0} <{0}>";

        public static CommandOption CreateOption(
            this CommandLineApplication command,
            string name,
            string description,
            CommandOptionType type,
            string template = DefaultTemplate
        )
        {
            string formatedTemplate = string.Format(template, name);
            return command.Option(formatedTemplate, description, type);
        }
    }
}