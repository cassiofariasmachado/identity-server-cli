using IdentityServer4.Models;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Commands.IdentityResources
{
    public class NewIdentityResourceCommand : ICommand
    {
        private const string Description = "Create an identity resource.";

        private readonly IConsole _console;

        private readonly IIdentityResourceRepository _identityResourceRepository;

        public NewIdentityResourceCommand(
            IConsole console,
            IIdentityResourceRepository identityResourceRepository
        )
        {
            _console = console;
            _identityResourceRepository = identityResourceRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            var disabled = command.AddResourceDisabled();
            var name = command.AddResourceName();
            var displayName = command.AddResourceDisplayName();
            var description = command.AddResourceDescription();
            var claims = command.AddResourceUserClaims().IsRequired();
            var emphasize = AddEmphasize(command);
            var required = AddRequired(command);
            var noShowInDiscoveryDocument = AddNoShowInDiscoveryDocument(command);

            command.OnExecute(async () =>
            {
                var identityResource = new IdentityResource(name.Value, claims.Values);

                if (disabled.HasValue())
                {
                    identityResource.Enabled = false;
                }

                if (displayName.HasValue())
                {
                    identityResource.DisplayName = displayName.Value();
                }

                if (description.HasValue())
                {
                    identityResource.Description = description.Value();
                }

                if (claims.HasValue())
                {
                    identityResource.UserClaims = claims.Values;
                }

                if (required.HasValue())
                {
                    identityResource.Required = true;
                }

                if (emphasize.HasValue())
                {
                    _console.WriteLine("has emphasize");
                    identityResource.Emphasize = true;
                }

                if (noShowInDiscoveryDocument.HasValue())
                {
                    identityResource.ShowInDiscoveryDocument = false;
                }

                await _identityResourceRepository.AddAsync(identityResource);

                _console.WriteSuccess("IdentityResource created.");
            });

        }

        private CommandOption AddRequired(CommandLineApplication command) =>
            command.CreateOption(
                nameof(IdentityResource.Required),
                "Specifies whether the user can de-select the scope on the consent screen. Defaults to false.",
                CommandOptionType.NoValue);

        private CommandOption AddEmphasize(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(IdentityResource.Emphasize),
                    "Specifies whether the consent screen will emphasize this scope. Defaults to false.",
                    CommandOptionType.NoValue);

        private CommandOption AddNoShowInDiscoveryDocument(CommandLineApplication command) =>
            command.CreateOption(
                $"No{nameof(IdentityResource.ShowInDiscoveryDocument)}",
                "Specifies whether this scope is shown in the discovery document. Defaults to true.",
                CommandOptionType.NoValue);

    }
}