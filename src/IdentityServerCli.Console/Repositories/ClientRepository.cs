using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerCli.Console.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerCli.Console.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public ClientRepository(IConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(Client client)
        {
            var clientEntity = client.ToEntity();

            await this._configurationDbContext.Clients.AddAsync(clientEntity);

            await this._configurationDbContext.SaveChangesAsync();
        }

        public async Task<IList<Client>> GetClientsAsync()
            => await this._configurationDbContext.Clients
                .OrderBy(c => c.ClientName)
                .Select(c => c.ToModel())
                .ToListAsync();
    }
}