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

        private readonly CommandLineApplication _commandLineApp;

        private const string CommandName = "new";

        private const string SubCommandName = "identity-resource";

        public NewIdentityResourceCommandTest()
        {
            this._console = A.Fake<IConsole>();
            this._identityResourceRepository = A.Fake<IIdentityResourceRepository>();
            var newIdentityResourceCmd = new NewIdentityResourceCommand(this._console, this._identityResourceRepository);
            this._commandLineApp = CommandLineApplicationUtils.CreateCommandLineApplication(
                    CommandName, SubCommandName, newIdentityResourceCmd
            );
        }

        [Theory]
        [InlineData("disabled-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceDisabled(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--disabled");

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && !i.Enabled);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceWithCorrectName(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims);

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" }, "Awesome identity resource")]
        public void ShouldCreateANewIdentityResourceWithDisplayName(string identityResourceName, string[] userClaims, string displayName)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--display-name", displayName);

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName && i.DisplayName == displayName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" }, "Awesome identity resource")]
        public void ShouldCreateANewIdentityResourceWithDescription(string identityResourceName, string[] userClaims, string description)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--description", description);

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName && i.Description == description);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email", "uniquename" })]
        public void ShouldCreateANewIdentityResourceWithUserClaims(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims);

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(
                i => i.Name == identityResourceName
                    && i.UserClaims.All(u => userClaims.Contains(u)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceWithEmphasize(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--emphasize");

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && i.Emphasize);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityResourceRequired(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--required");

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && i.Required);
            SuccessMessageMustHaveHappened();
        }


        [Theory]
        [InlineData("awesome-identity-resource", new[] { "email" })]
        public void ShouldCreateANewIdentityDidntShowInDiscoveryDocument(string identityResourceName, string[] userClaims)
        {
            var args = CreateArguments(identityResourceName, userClaims, "--no-show-in-discovery-document");

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithIdentityResourceThat(i => i.Name == identityResourceName && !i.ShowInDiscoveryDocument);
            SuccessMessageMustHaveHappened();
        }

        private void AddAsyncMustHaveHappenedWithIdentityResourceThat(Expression<Func<IdentityResource, bool>> predicate)
        {
            A.CallTo(() => this._identityResourceRepository.AddAsync(A<IdentityResource>.That.Matches(predicate)))
                .MustHaveHappened();
        }

        private void SuccessMessageMustHaveHappened()
        {
            A.CallTo(() => this._console.Out.WriteLine("Identity resource created."))
                .MustHaveHappened();
        }

        private string[] CreateArguments(string identityResourceName, string[] userClaims, params string[] args)
        {
            var mainArgs = new[] { CommandName, SubCommandName, identityResourceName };
            var userClaimsArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments("--user-claims", userClaims);

            return mainArgs.Concat(userClaimsArgs)
                .Concat(args)
                .ToArray();
        }
    }
}