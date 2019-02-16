using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerCli.Console.Interfaces.Repositories;

namespace IdentityServerCli.Console.Repositories
{
    public class IdentityResourceRepository : IIdentityResourceRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public IdentityResourceRepository(IConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(IdentityResource identityResource)
        {
            var identityResourceEntity = identityResource.ToEntity();

            await _configurationDbContext.IdentityResources.AddAsync(identityResourceEntity);

            await _configurationDbContext.SaveChangesAsync();
        }
    }
}