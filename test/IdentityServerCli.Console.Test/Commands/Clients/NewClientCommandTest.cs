using System;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.Clients;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.Clients
{
    public class NewClientCommandTest
    {
        private readonly IConsole _console;

        private readonly IClientRepository _clientRepository;

        private const string CommandName = "new";

        private const string SubCommandName = "client";

        public NewClientCommandTest()
        {
            _console = A.Fake<IConsole>();
            _clientRepository = A.Fake<IClientRepository>();
        }

        [Theory]
        [InlineData("disabled-client")]
        public void ShouldCreateANewClientDisabled(string clientId)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--disabled");

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && !c.Enabled);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client")]
        public void ShouldCreateANewClientWithCorrectId(string clientId)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId);

            AddAsyncMustHaveHappenedWithApiResourceThat(c => c.ClientId == clientId);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "JavaScript Client")]
        public void ShouldCreateANewClientWithClientName(string clientId, string clientName)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--client-name", clientName);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.ClientName == clientName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "A JavaScript client.")]
        public void ShouldCreateANewClientWithDescription(string clientId, string description)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--description", description);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.Description == description);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "https://javascript-client.com")]
        public void ShouldCreateANewClientWithClientUri(string clientId, string clientUri)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--client-uri", clientUri);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.ClientUri == clientUri);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "https://javascript-client.com/logo.png")]
        public void ShouldCreateANewClientWithLogoUri(string clientId, string logoUri)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--logo-uri", logoUri);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.LogoUri == logoUri);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "js-client-hash")]
        public void ShouldCreateANewClientWithClientSecrets(string clientId, string secret)
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--client-secrets", secret);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.ClientSecrets.Any(s => s.Value == secret.Sha256()));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "js-client-hash", "sha256")]
        [InlineData("js-client", "js-client-hash", "sha512")]
        [InlineData("js-client", "js-client-hash", "invalid-algo")]
        public void ShouldCreateANewClientWithClientSecretsAndSecretAlgorithm(
            string clientId,
            string secret,
            string algo
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--client-secrets", secret, "--secret-algorithm", algo);

            Func<Secret, bool> validateSecret = (s) =>
            {
                Func<string, string> sha256 = (str) => secret.Sha256();
                Func<string, string> sha512 = (str) => secret.Sha512();

                Func<string, string> encodeFunc = algo == "sha512" ? sha512 : sha256;

                return s.Value == encodeFunc(secret);
            };

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.ClientSecrets.Any(validateSecret));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", "js-client-hash", "invalid-algo")]
        [InlineData("js-client", "js-client-hash", "md5")]
        public void ShouldCreateANewClientAndWriteWarningWhenSecretAlgorithmIsInvalid(
            string clientId,
            string secret,
            string algo
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            app.Execute(CommandName, SubCommandName, clientId, "--client-secrets", secret, "--secret-algorithm", algo);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId && c.ClientSecrets.Any(s => s.Value == secret.Sha256()));
            SuccessMessageMustHaveHappened();
            WarningAboutAlgorithmMustHaveHappened(algo);
        }

        [Theory]
        [InlineData("js-client", new[] { "password", "implicit" })]
        public void ShouldCreateANewClientWithAllowedGrantTypes(
            string clientId,
            string[] allowedGrantTypes
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            var allowedGrantTypesArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments(
                    "--allowed-grant-types", allowedGrantTypes)
                .ToArray();
            var args = CreateArguments(clientId, allowedGrantTypesArgs);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId
                    && c.AllowedGrantTypes.All(g => allowedGrantTypes.Contains(g)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", new[] { "http://localhost:5000", "http://localhost:3000" })]
        public void ShouldCreateANewClientWithRedirectUris(
            string clientId,
            string[] redirectUris
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            var redirectUrisArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments(
                    "--redirect-uris", redirectUris)
                .ToArray();
            var args = CreateArguments(clientId, redirectUrisArgs);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId
                    && c.RedirectUris.All(r => redirectUris.Contains(r)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", new[] { "http://localhost:5000/logout", "http://localhost:3000/logout" })]
        public void ShouldCreateANewClientWithPostLogoutRedirectUris(
            string clientId,
            string[] postLogoutRedirectUris
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            var postLogoutRedirectUrisArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments(
                    "--post-logout-redirect-uris", postLogoutRedirectUris)
                .ToArray();
            var args = CreateArguments(clientId, postLogoutRedirectUrisArgs);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId
                    && c.RedirectUris.All(r => postLogoutRedirectUrisArgs.Contains(r)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", new[] { "openid", "profile-api" })]
        public void ShouldCreateANewClientWithAllowedScopes(
            string clientId,
            string[] allowedScopes
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            var allowedScopesArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments(
                    "--allowed-scopes", allowedScopes)
                .ToArray();
            var args = CreateArguments(clientId, allowedScopesArgs);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId
                    && c.AllowedScopes.All(r => allowedScopesArgs.Contains(r)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("js-client", new[] { "http://localhost:5000", "http://localhost:3000" })]
        public void ShouldCreateANewClientWithAllowedCorsOrigins(
            string clientId,
            string[] allowedCorsOrigins
        )
        {
            var newClientCommand = new NewClientCommand(_console, _clientRepository);

            var app = CreateCommandLineApplication(newClientCommand);

            var allowedCorsOriginsArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments(
                    "--allowed-cors-origins", allowedCorsOrigins)
                .ToArray();
            var args = CreateArguments(clientId, allowedCorsOriginsArgs);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                c => c.ClientId == clientId
                    && c.AllowedCorsOrigins.All(r => allowedCorsOriginsArgs.Contains(r)));
            SuccessMessageMustHaveHappened();
        }

        private void AddAsyncMustHaveHappenedWithApiResourceThat(Expression<Func<Client, bool>> predicate)
        {
            A.CallTo(() =>
                    _clientRepository.AddAsync(
                        A<Client>.That.Matches(predicate)))
                .MustHaveHappened();
        }

        private void SuccessMessageMustHaveHappened()
        {
            A.CallTo(() => _console.Out.WriteLine("Client created."))
                .MustHaveHappened();
        }

        private void WarningAboutAlgorithmMustHaveHappened(string algorithm)
        {
            A.CallTo(() =>
                _console.Out.WriteLine(
                        $"Warning: Not found algorithm {algorithm}, It'll be used sha256 instead."
                    ))
                .MustHaveHappened();
        }

        private string[] CreateArguments(string clientId, params string[] args)
        {
            var mainArgs = new[] {
                CommandName, SubCommandName, clientId
            };

            return mainArgs.Concat(args)
                .ToArray();
        }

        private CommandLineApplication CreateCommandLineApplication(NewClientCommand newClientCommand)
            => CommandLineApplicationUtils.CreateCommandLineApplication(
                    CommandName,
                    SubCommandName,
                    newClientCommand);
    }
}