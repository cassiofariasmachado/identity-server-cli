using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Commands.Clients
{
    public class NewClientCommand : ICommand
    {
        private const string Description = "Create a client.";

        private readonly IConsole _console;

        private readonly IClientRepository _clientRepository;

        public NewClientCommand(
            IConsole console, IClientRepository clientRepository
        )
        {
            this._console = console;
            this._clientRepository = clientRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            var disabled = AddDisabled(command);
            var clientId = AddClientId(command);
            var clientName = AddClientName(command);
            var clientUri = AddClientUri(command);
            var logoUri = AddLogoUri(command);
            var clientSecrets = AddClientSecrets(command);
            var secretAlgorithm = AddSecretAlgorithm(command);
            var allowedGrantTypes = AddAllowedGrantTypes(command);
            var redirectUris = AddRedirectUris(command);
            var postLogoutRedirectUris = AddPostLogoutRedirectUris(command);
            var allowedScopes = AddAllowedScopes(command);
            var allowedCorsOrigins = AddAllowedCorsOrigins(command);

            command.OnExecute(async () =>
            {
                var client = new Client();

                if (disabled.HasValue())
                {
                    client.Enabled = false;
                }

                client.ClientId = clientId.Value;

                if (clientName.HasValue())
                {
                    client.ClientName = clientName.Value();
                }

                if (clientUri.HasValue())
                {
                    client.ClientUri = clientUri.Value();
                }

                if (logoUri.HasValue())
                {
                    client.LogoUri = logoUri.Value();
                }

                if (clientSecrets.HasValue())
                {
                    var encodeFunc = GetSecretEncodeFunc(secretAlgorithm);

                    var mappedClientSecrets = clientSecrets.Values.Select(encodeFunc)
                        .Select(s => new Secret(s))
                        .ToList();

                    client.ClientSecrets = mappedClientSecrets;
                }

                if (allowedGrantTypes.HasValue())
                {
                    client.AllowedGrantTypes = allowedGrantTypes.Values;
                }

                if (redirectUris.HasValue())
                {
                    client.RedirectUris = redirectUris.Values;
                }

                if (postLogoutRedirectUris.HasValue())
                {
                    client.PostLogoutRedirectUris = postLogoutRedirectUris.Values;
                }

                if (allowedScopes.HasValue())
                {
                    client.AllowedScopes = allowedScopes.Values;
                }

                if (allowedCorsOrigins.HasValue())
                {
                    client.AllowedCorsOrigins = allowedCorsOrigins.Values;
                }

                await this._clientRepository.AddAsync(client);

                this._console.WriteSuccess("Client created.");
            });
        }

        private CommandOption AddDisabled(CommandLineApplication command) =>
            command.CreateOption(
                    "disabled",
                    "Indicates if this client is disabled. Defaults to enabled.",
                    CommandOptionType.NoValue);

        private CommandArgument AddClientId(CommandLineApplication command) =>
            command.Argument(
                    nameof(Client.ClientId),
                    "Unique ID of the client.")
                .IsRequired();

        private CommandOption AddClientName(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Client.ClientName),
                    "Client display name (used for logging and consent screen).",
                    CommandOptionType.SingleValue);

        private CommandOption AddClientUri(CommandLineApplication command) =>
            command.CreateOption(
                nameof(Client.ClientUri),
                "URI to further information about client (used on consent screen).",
                CommandOptionType.SingleValue);

        private CommandOption AddLogoUri(CommandLineApplication command) =>
            command.CreateOption(
                nameof(Client.LogoUri),
                "URI to client logo (used on consent screen).",
                CommandOptionType.SingleValue);

        private CommandOption AddClientSecrets(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Client.ClientSecrets),
                    "Client secrets - only relevant for flows that require a secret.",
                    CommandOptionType.MultipleValue);

        private CommandOption AddSecretAlgorithm(CommandLineApplication command) =>
            command.CreateOption(
                    "SecretAlgorithm",
                    "The algorithm used to encode the client secrets, can be \"sha256\" or \"sha512\". Defaults to sha256.",
                    CommandOptionType.SingleValue);

        private CommandOption AddAllowedGrantTypes(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Client.AllowedGrantTypes),
                    "Specifies the allowed grant types (legal combinations of AuthorizationCode, Implicit, Hybrid, ResourceOwner, ClientCredentials).",
                    CommandOptionType.MultipleValue);

        private CommandOption AddRedirectUris(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Client.RedirectUris),
                    "Specifies allowed URIs to return tokens or authorization codes to.",
                    CommandOptionType.MultipleValue);

        private CommandOption AddPostLogoutRedirectUris(CommandLineApplication command) =>
            command.CreateOption(
                    nameof(Client.PostLogoutRedirectUris),
                    "Specifies allowed URIs to redirect to after logout.",
                    CommandOptionType.MultipleValue);

        private CommandOption AddAllowedScopes(CommandLineApplication command) =>
            command.CreateOption(
                nameof(Client.AllowedScopes),
                "Specifies the api scopes that the client is allowed to request. If empty, the client can't access any scope.",
                CommandOptionType.MultipleValue);

        private CommandOption AddAllowedCorsOrigins(CommandLineApplication command) =>
            command.CreateOption(
                nameof(Client.AllowedCorsOrigins),
                "The allowed CORS origins for JavaScript clients.",
                CommandOptionType.MultipleValue);

        private Func<string, string> GetSecretEncodeFunc(CommandOption secretsAlgorithm)
        {
            bool hasInformedAlgorithm = secretsAlgorithm.HasValue();
            Func<string, string> sha256 = (secret) => secret.Sha256();
            Func<string, string> sha512 = (secret) => secret.Sha512();

            if (hasInformedAlgorithm)
            {
                var algorithm = secretsAlgorithm.Value();

                switch (algorithm)
                {
                    case ("sha512"):
                        return sha512;
                    case ("sha256"):
                        return sha256;
                    default:
                        _console.WriteWarning(
                            $"Warning: Not found algorithm {algorithm}, It'll be used sha256 instead."
                        );
                        return sha256;
                }
            }

            return sha256;
        }
    }
}