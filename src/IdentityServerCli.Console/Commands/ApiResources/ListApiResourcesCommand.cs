using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServerCli.Console.Interfaces.Repositories;
using McMaster.Extensions.CommandLineUtils;
using IdentityServerCli.Console.Extensions;
using System.Linq;

namespace IdentityServerCli.Console.Commands.ApiResources
{
    public class ListApiResourcesCommand : ICommand
    {
        private const string Description = "List api resources.";

        private readonly IConsole _console;

        private readonly IApiResourceRepository _apiResourceRepository;

        public ListApiResourcesCommand(IConsole console, IApiResourceRepository apiResourceRepository)
        {
            this._console = console;
            this._apiResourceRepository = apiResourceRepository;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = Description;

            command.OnExecute(async () =>
            {
                var apiRespources = await this._apiResourceRepository.GetApiResourcesAsync();

                if (!apiRespources.Any())
                {
                    this._console.WriteLine("There aren't api resources.");
                    return;
                }

                var headers = new List<string>
                {
                    nameof(ApiResource.Name),
                    nameof(ApiResource.DisplayName),
                    nameof(ApiResource.Description),
                    nameof(ApiResource.Enabled),
                };

                this._console.WriteTable(headers, apiRespources, apiResource => new List<string>
                {
                    apiResource.Name,
                    apiResource.DisplayName,
                    apiResource.Description,
                    apiResource.Enabled.ToString()
                });
            });
        }
    }
}