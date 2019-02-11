using IdentityServer4.Models;
using McMaster.Extensions.CommandLineUtils;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Repositories;
using System;

namespace IdentityServerCli.Console.Commands.ApiResources
{
    public class NewApiResourceCommand : ICommand
    {
        private readonly IConsole _console;

        private readonly ApiResourceRepository _apiResourceRepository;

        public NewApiResourceCommand(
            IConsole console,
            ApiResourceRepository apiResourceRepository
        )
        {
            _console = console;
            _apiResourceRepository = apiResourceRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            var name = AddNameArgument(command);
            var displayName = AddDisplayNameOption(command);
            var description = AddDescription(command);

            command.OnExecute(async () =>
            {
                var apiResource = new ApiResource(name.Value);

                if (displayName.HasValue())
                {
                    apiResource.DisplayName = displayName.Value();
                }

                if (description.HasValue())
                {
                    apiResource.Description = description.Value();
                }

                _console.ForegroundColor = ConsoleColor.Green;
                _console.WriteLine();

                await _apiResourceRepository.AddAsync(apiResource);

                _console.WriteSuccess("ApiResource created.");
            });
        }

        private CommandArgument AddNameArgument(CommandLineApplication command) =>
                command.Argument(
                    nameof(ApiResource.Name).Dashrialize(),
                    "The unique name of the resource.")
                .IsRequired();

        private CommandOption AddDisplayNameOption(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(ApiResource.DisplayName).Dashrialize(),
                    "Display name of the resource.",
                    CommandOptionType.SingleValue);

        private CommandOption AddDescription(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(ApiResource.Description).Dashrialize(),
                    "Description of the resource.",
                    CommandOptionType.SingleValue);

    }
}