using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.Clients;
using IdentityServerCli.Console.Interfaces.Repositories;
using IdentityServerCli.Console.Test.Utils;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Commands.Clients
{
    public class ListClientsCommandTest
    {
        private readonly IConsole _console;

        private readonly IClientRepository _clientRepository;

        private readonly CommandLineApplication _commandLineApp;

        private readonly List<Client> _clients = new List<Client>
        {
            new Client {
                ClientId = "client",
                ClientName = "Client",
                Enabled = true
            }
        };

        private const string CommandName = "ls";

        private const string SubCommandName = "client";

        public ListClientsCommandTest()
        {
            this._console = A.Fake<IConsole>();
            this._clientRepository = A.Fake<IClientRepository>();
            var listClientCmd = new ListClientsCommand(this._console, this._clientRepository);
            this._commandLineApp = CommandLineApplicationUtils.CreateCommandLineApplication(
                CommandName, SubCommandName, listClientCmd
            );
        }

        [Fact]
        public void ShouldInformThatThereArentApiResources()
        {
            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            A.CallTo(() => this._console.Out.WriteLine("There aren't clients."))
                .MustHaveHappened();
        }

        [Fact]
        public void ShouldListAllApiResources()
        {
            A.CallTo(() => this._clientRepository.GetClientsAsync())
                .Returns(this._clients);

            var args = CreateArguments();

            this._commandLineApp.Execute(args);

            var outputs = new List<string> {
                "-------------------------------------",
                "|ClientId   |ClientName |Enabled    |",
                "-------------------------------------",
                "|client     |Client     |True       |",
                "-------------------------------------",
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