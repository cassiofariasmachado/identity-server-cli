using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.ApiResources;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.ApiResources
{
    public class ListApiResourcesCommandTest
    {
        private readonly IConsole _console;

        private readonly IApiResourceRepository _apiResourceRepository;

        private readonly CommandLineApplication _commandLineApp;

        private readonly List<ApiResource> _apiResources = new List<ApiResource>
        {
            new ApiResource {
                Name = "api-resource",
                DisplayName = "Api Resource",
                Description = "An amazing api resource.",
                Enabled = true
            }
        };

        private const string CommandName = "ls";

        private const string SubCommandName = "api-resource";

        public ListApiResourcesCommandTest()
        {
            this._console = A.Fake<IConsole>();
            this._apiResourceRepository = A.Fake<IApiResourceRepository>();
            var listApiResourceCmd = new ListApiResourcesCommand(this._console, this._apiResourceRepository);
            this._commandLineApp = CommandLineApplicationUtils.CreateCommandLineApplication(
                CommandName, SubCommandName, listApiResourceCmd
            );
        }

        [Fact]
        public void ShouldInformThatThereArentApiResources()
        {
            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            A.CallTo(() => this._console.Out.WriteLine("There aren't api resources."))
                .MustHaveHappened();
        }

        [Fact]
        public void ShouldListAllApiResources()
        {
            A.CallTo(() => this._apiResourceRepository.GetApiResourcesAsync())
                .Returns(this._apiResources);

            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            var outputs = new List<string> {
                "---------------------------------------------------------------------------------------------------------",
                "|Name                     |DisplayName              |Description              |Enabled                  |",
                "---------------------------------------------------------------------------------------------------------",
                "|api-resource             |Api Resource             |An amazing api resource. |True                     |",
                "---------------------------------------------------------------------------------------------------------",
            };

            outputs.Select(output => A.CallTo(() => this._console.Out.WriteLine(output)))
                .ToList()
                .ForEach(cfg => cfg.MustHaveHappened());
        }

        private string[] CreateArguments(params string[] args)
            => new[] { CommandName, SubCommandName }
                .Concat(args)
                .ToArray();
    }
}