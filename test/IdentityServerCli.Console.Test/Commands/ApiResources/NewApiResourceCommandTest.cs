using System;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.ApiResources;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.ApiResources
{
    public class NewApiResourceCommandTest
    {
        private readonly IConsole _console;

        private readonly IApiResourceRepository _apiResourceRepository;

        private readonly CommandLineApplication _commandLineApp;

        private const string CommandName = "new";

        private const string SubCommandName = "api-resource";

        public NewApiResourceCommandTest()
        {
            this._console = A.Fake<IConsole>();
            this._apiResourceRepository = A.Fake<IApiResourceRepository>();
            var newApiResourceCmd = new NewApiResourceCommand(this._console, this._apiResourceRepository);
            this._commandLineApp = CommandLineApplicationUtils.CreateCommandLineApplication(
                CommandName, SubCommandName, newApiResourceCmd
            );
        }

        [Theory]
        [InlineData("disabled-api")]
        public void ShouldCreateANewApiResourceDisabled(string apiResourceName)
        {
            this._commandLineApp.Execute(CommandName, SubCommandName, apiResourceName, "--disabled");

            AddAsyncMustHaveHappenedWithApiResourceThat(a => a.Name == apiResourceName && !a.Enabled);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("microservice-api")]
        [InlineData("awesome-api")]
        public void ShouldCreateANewApiResourceWithCorrectName(string apiResourceName)
        {
            this._commandLineApp.Execute(CommandName, SubCommandName, apiResourceName);

            AddAsyncMustHaveHappenedWithApiResourceThat(a => a.Name == apiResourceName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("microservice-api", "Microservice API")]
        public void ShouldCreateANewApiResourceWithDisplayName(string apiResourceName, string displayName)
        {
            this._commandLineApp.Execute(CommandName, SubCommandName, apiResourceName, "--display-name", displayName);

            AddAsyncMustHaveHappenedWithApiResourceThat(a => a.Name == apiResourceName && a.DisplayName == displayName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("microservice-api", "A really micro service.")]
        public void ShouldCreateANewApiResourceWithDescription(string apiResourceName, string description)
        {
            this._commandLineApp.Execute(CommandName, SubCommandName, apiResourceName, "--description", description);

            AddAsyncMustHaveHappenedWithApiResourceThat(a => a.Name == apiResourceName && a.Description == description);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("api-with-claims", new string[] { "email" })]
        [InlineData("api-with-claims", new string[] { "email", "role" })]
        public void ShouldCreateANewApiResourceWithTheInformedClaims(string apiResourceName, string[] claims)
        {
            var mainArgs = new string[] {
                CommandName, SubCommandName, apiResourceName
            };
            var userClaimsArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments("--user-claims", claims);

            var args = mainArgs.Concat(userClaimsArgs).ToArray();

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                a => a.Name == apiResourceName
                    && a.UserClaims.All(c => claims.Contains(c)));
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("api-with-scopes", new string[] { "profile" })]
        [InlineData("api-with-scopes", new string[] { "profile", "contact" })]
        public void ShouldCreateANewApiResourceWithTheInformedScopes(string apiResourceName, string[] scopes)
        {
            var mainArgs = new string[] {
                CommandName, SubCommandName, apiResourceName
            };
            var scopesArgs = CommandLineApplicationUtils.CreateMultipleOptionArguments("--scopes", scopes);

            var args = mainArgs.Concat(scopesArgs).ToArray();

            this._commandLineApp.Execute(args);

            AddAsyncMustHaveHappenedWithApiResourceThat(
                a => a.Name == apiResourceName
                    && a.Scopes.All(s => scopes.Contains(s.Name)));
            SuccessMessageMustHaveHappened();
        }

        private void AddAsyncMustHaveHappenedWithApiResourceThat(Expression<Func<ApiResource, bool>> predicate)
        {
            A.CallTo(() => this._apiResourceRepository.AddAsync(A<ApiResource>.That.Matches(predicate)))
                .MustHaveHappened();
        }

        private void SuccessMessageMustHaveHappened()
        {
            A.CallTo(() => _console.Out.WriteLine("Api resource created."))
                .MustHaveHappened();
        }
    }
}