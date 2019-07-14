using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Entities;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Commands.Clients
{
    public class ListClientsCommand : ICommand
    {
        private const string Description = "List clients.";

        private readonly IConsole _console;

        private readonly IClientRepository _clientRepository;

        public ListClientsCommand(IConsole console, IClientRepository clientRepository)
        {
            this._console = console;
            this._clientRepository = clientRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            command.OnExecute(async () =>
            {
                var clients = await this._clientRepository.GetClientsAsync();

                if (!clients.Any())
                {
                    this._console.WriteLine("There aren't clients.");
                    return;
                }

                var headers = new List<string>
                {
                    nameof(Client.ClientId),
                    nameof(Client.ClientName),
                    nameof(Client.Enabled),
                };

                this._console.WriteTable(headers, clients, client => new List<string>
                {
                    client.ClientId,
                    client.ClientName,
                    client.Enabled.ToString()
                });
            });
        }
    }
}