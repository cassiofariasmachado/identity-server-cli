using IdentityServer4.Models;
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
            string optionName = name.Dashrialize();
            string formatedTemplate = string.Format(template, optionName);
            return command.Option(formatedTemplate, description, type);
        }

        public static CommandOption AddResourceDisabled(this CommandLineApplication command) =>
            command.CreateOption(
                    "disabled",
                    "Indicates if this resource is enabled. Defaults to true.",
                    CommandOptionType.NoValue);

        public static CommandArgument AddResourceNameArgument(this CommandLineApplication command) =>
            command.Argument(
                    nameof(Resource.Name),
                    "The unique name of the resource.")
                .IsRequired();

        public static CommandOption AddResourceDisplayNameOption(this CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Resource.DisplayName),
                    "Display name of the resource.",
                    CommandOptionType.SingleValue);

        public static CommandOption AddResourceDescription(this CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Resource.Description),
                    "Description of the resource.",
                    CommandOptionType.SingleValue);

        public static CommandOption AddResourceUserClaims(this CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Resource.UserClaims),
                    "List of accociated user claims that should be included when this resource is requested.",
                    CommandOptionType.MultipleValue);

    }
}