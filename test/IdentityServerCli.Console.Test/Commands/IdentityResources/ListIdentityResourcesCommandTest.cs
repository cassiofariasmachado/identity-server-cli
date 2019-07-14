using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.IdentityResources;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.IdentityResources
{
    public class ListIdentityResourcesCommandTest
    {
        private readonly IConsole _console;

        private readonly IIdentityResourceRepository _identityResourceRepository;

        private readonly CommandLineApplication _commandLineApp;

        private readonly List<IdentityResource> _identityResources = new List<IdentityResource>
        {
            new IdentityResource {
                Name = "identity-resource",
                DisplayName = "Identity Resource",
                Description = "An amazing identity resource.",
                Enabled = true
            }
        };

        private const string CommandName = "ls";

        private const string SubCommandName = "identity-resource";

        public ListIdentityResourcesCommandTest()
        {
            this._console = A.Fake<IConsole>();
            this._identityResourceRepository = A.Fake<IIdentityResourceRepository>();
            var listIdentityResourcesCmd = new ListIdentityResourcesCommand(this._console, this._identityResourceRepository);
            this._commandLineApp = CommandLineApplicationUtils.CreateCommandLineApplication(
                CommandName, SubCommandName, listIdentityResourcesCmd
            );
        }

        [Fact]
        public void ShouldInformThatThereArentIdentityResources()
        {
            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            A.CallTo(() => this._console.Out.WriteLine("There aren't identity resources."))
                .MustHaveHappened();
        }

        [Fact]
        public void ShouldListAllIdentityResources()
        {
            A.CallTo(() => this._identityResourceRepository.GetIdentityResourcesAsync())
                .Returns(this._identityResources);

            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            var outputs = new List<string> {
                "-----------------------------------------------------------------------------------------------------------------------------",
                "|Name                          |DisplayName                   |Description                   |Enabled                       |",
                "-----------------------------------------------------------------------------------------------------------------------------",
                "|identity-resource             |Identity Resource             |An amazing identity resource. |True                          |",
                "-----------------------------------------------------------------------------------------------------------------------------",
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