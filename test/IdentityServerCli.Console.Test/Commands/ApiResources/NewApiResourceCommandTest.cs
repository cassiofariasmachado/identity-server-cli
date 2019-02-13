using System;
using System.Linq.Expressions;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.ApiResources;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.ApiResources
{
    public class NewApiResourceCommandTest
    {
        private readonly IConsole _console;

        private readonly IApiResourceRepository _apiResourceRepository;

        private const string CommandName = "new";

        private const string SubCommandName = "api-resource";

        public NewApiResourceCommandTest()
        {
            _console = A.Fake<IConsole>();
            _apiResourceRepository = A.Fake<IApiResourceRepository>();
        }

        [Theory]
        [InlineData("microservice-api")]
        [InlineData("awesome-api")]
        public void ShouldCreateANewApiResourceWithCorrectName(string apiResourceName)
        {
            var newApiResourceCommand = new NewApiResourceCommand(_console, _apiResourceRepository);

            var app = CreateCommandLineApplication(newApiResourceCommand);

            app.Execute(CommandName, SubCommandName, apiResourceName);

            AddAsyncMustHaveHappenedThatApiResource(a => a.Name == apiResourceName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("microservice-api", "Microservice API")]
        public void ShouldCreateANewApiResourceWithDisplayName(string apiResourceName, string displayName)
        {
            var newApiResourceCommand = new NewApiResourceCommand(_console, _apiResourceRepository);

            var app = CreateCommandLineApplication(newApiResourceCommand);

            app.Execute(CommandName, SubCommandName, apiResourceName, "--display-name", displayName);

            AddAsyncMustHaveHappenedThatApiResource(a => a.Name == apiResourceName && a.DisplayName == displayName);
            SuccessMessageMustHaveHappened();
        }

        [Theory]
        [InlineData("microservice-api", "A really micro service.")]
        public void ShouldCreateANewApiResourceWithDescription(string apiResourceName, string description)
        {
            var newApiResourceCommand = new NewApiResourceCommand(_console, _apiResourceRepository);

            var app = CreateCommandLineApplication(newApiResourceCommand);

            app.Execute(CommandName, SubCommandName, apiResourceName, "--description", description);

            AddAsyncMustHaveHappenedThatApiResource(a => a.Name == apiResourceName && a.Description == description);
            SuccessMessageMustHaveHappened();
        }


        private void AddAsyncMustHaveHappenedThatApiResource(Expression<Func<ApiResource, bool>> predicate)
        {
            A.CallTo(() =>
                    _apiResourceRepository.AddAsync(
                        A<ApiResource>.That.Matches(predicate)))
                .MustHaveHappened();
        }

        private void SuccessMessageMustHaveHappened()
        {
            A.CallTo(() => _console.Out.WriteLine("ApiResource created."))
                .MustHaveHappened();
        }

        private CommandLineApplication CreateCommandLineApplication(NewApiResourceCommand newApiResourceCommand)
        {
            var app = new CommandLineApplication();

            app.Command(CommandName, newCmd =>
            {
                newCmd.Command(SubCommandName, newApiResourceCommand.Execute);
            });

            return app;
        }
    }
}