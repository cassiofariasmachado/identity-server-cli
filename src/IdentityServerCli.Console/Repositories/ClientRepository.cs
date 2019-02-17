using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerCli.Console.Interfaces.Repositories;

namespace IdentityServerCli.Console.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public ClientRepository(IConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(Client client)
        {
            var clientEntity = client.ToEntity();

            await _configurationDbContext.Clients.AddAsync(clientEntity);

            await _configurationDbContext.SaveChangesAsync();
        }
    }
}