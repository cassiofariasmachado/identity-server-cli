using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Commands.IdentityResources
{
    public class ListIdentityResourcesCommand : ICommand
    {
        private const string Description = "List identity resources.";

        private readonly IConsole _console;

        private readonly IIdentityResourceRepository _identityResourceRepository;

        public ListIdentityResourcesCommand(IConsole console, IIdentityResourceRepository identityResourceRepository)
        {
            this._console = console;
            this._identityResourceRepository = identityResourceRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            command.OnExecute(async () =>
            {
                var identityResources = await this._identityResourceRepository.GetIdentityResourcesAsync();

                if (!identityResources.Any())
                {
                    this._console.WriteLine("There aren't identity resources.");
                    return;
                }

                var headers = new List<string>
                {
                    nameof(IdentityResource.Name),
                    nameof(IdentityResource.DisplayName),
                    nameof(IdentityResource.Description),
                    nameof(IdentityResource.Enabled),
                };

                this._console.WriteTable(headers, identityResources, identityResource => new List<string>
                {
                    identityResource.Name,
                    identityResource.DisplayName,
                    identityResource.Description,
                    identityResource.Enabled.ToString()
                });
            });

        }
    }
}