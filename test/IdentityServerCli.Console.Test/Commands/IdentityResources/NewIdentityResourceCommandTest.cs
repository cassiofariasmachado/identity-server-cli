using System;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.IdentityResources;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.IdentityResources
{
    public class NewIdentityResourceCommandTest
    {
        private readonly IConsole _console;

        private readonly IIdentityResourceRepository _identityResourceRepository;

        private const string CommandName = "new";

        private const string SubCommandName = "identity-resource";

        public NewIdentityResourceCommandTest()
        {
            _console = A.Fake<IConsole>();
            _identityResourceRepository = A.Fake<IIdentityResourceRepository>();
        }

        [Theory]
        [InlineData("disabled-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceDisabled(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--disabled");

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && !i.Enabled);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceWithCorrectName(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" }, "Awesome identity resource")]
        public void ShouldCreateANewIdentityResourceWithDisplayName(string identityResourceName, string[] userClaims, string displayName)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--display-name", displayName);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName && i.DisplayName == displayName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" }, "Awesome identity resource")]
        public void ShouldCreateANewIdentityResourceWithDescription(string identityResourceName, string[] userClaims, string description)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--description", description);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName && i.Description == description);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email", "uniquename" })]
        public void ShouldCreateANewIdentityResourceWithUserClaims(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims);

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName
                    && i.UserClaims.All(u => userClaims.Contains(u)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceWithEmphasize(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--emphasize");

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && i.Emphasize);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceRequired(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--required");

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && i.Required);
            SuccessMessageMustHaveHappened();
        }


        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityDidntShowInDiscoveryDocument(string identityResourceName, string[] userClaims)
        {
            var newIdentityResourceCommand = new NewIdentityResourceCommand(_console, _identityResourceRepository);

            var app = CreateCommandLineApplication(newIdentityResourceCommand);

            var args = CreateArguments(identityResourceName, userClaims, "--no-show-in-discovery-document");

            app.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && !i.ShowInDiscoveryDocument);
            SuccessMessageMustHaveHappened();
        }

        private void AddAsyncMustHaveHappenedWithIdentityResourceThat(Expression<Func<IdentityResource, bool>> predicate)
        {
            A.CallTo(() =>
                    _identityResourceRepository.AddAsync(
                        A<IdentityResource>.That.Matches(predicate)))
                .MustHaveHappened();
        }

        private void SuccessMessageMustHaveHappened()
        {
            A.CallTo(() => _console.Out.WriteLine("IdentityResource created."))
                .MustHaveHappened();
        }

        private string[] CreateArguments(string identityResourceName, string[] userClaims, params string[] args)
        {
            var mainArgs = new[] {
                CommandName, SubCommandName, identityResourceName
            };
            var userClaimsArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments("--user-claims", userClaims);

            return mainArgs.Concat(userClaimsArgs)
                .Concat(args)
                .ToArray();
        }

        private CommandLineApplication CreateCommandLineApplication(NewIdentityResourceCommand newApiResourceCommand)
            => CommandLineApplicationUtils.CreateCommandLineApplication(
                    CommandName,
                    SubCommandName,
                    newApiResourceCommand);

    }
}