using IdentityServer4.Models;
using McMaster.Extensions.CommandLineUtils;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Interfaces.Repositories;
using System.Linq;

namespace IdentityServerCli.Console.Commands.ApiResources
{
    public class NewApiResourceCommand : ICommand
    {
        private const string Description = "Create an api resource.";

        private readonly IConsole _console;

        private readonly IApiResourceRepository _apiResourceRepository;

        public NewApiResourceCommand(
            IConsole console,
            IApiResourceRepository apiResourceRepository
        )
        {
            this._console = console;
            this._apiResourceRepository = apiResourceRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            var disabled = command.AddResourceDisabled();
            var name = command.AddResourceName();
            var displayName = command.AddResourceDisplayName();
            var description = command.AddResourceDescription();
            var claims = command.AddResourceUserClaims();
            var scopes = AddScopes(command);

            command.OnExecute(async () =>
            {
                var apiResource = new ApiResource(name.Value);

                if (disabled.HasValue())
                {
                    apiResource.Enabled = false;
                }

                if (displayName.HasValue())
                {
                    apiResource.DisplayName = displayName.Value();
                }

                if (description.HasValue())
                {
                    apiResource.Description = description.Value();
                }

                if (scopes.HasValue())
                {
                    var mappedScopes = scopes.Values
                        .Select(value => new Scope(value))
                        .ToList();

                    apiResource.Scopes = mappedScopes;
                }

                if (claims.HasValue())
                {
                    apiResource.UserClaims = claims.Values;
                }

                await this._apiResourceRepository.AddAsync(apiResource);

                this._console.WriteSuccess("Api resource created.");
            });
        }

        private CommandOption AddScopes(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(ApiResource.Scopes),
                    "The scopes of API",
                    CommandOptionType.MultipleValue);
    }
}