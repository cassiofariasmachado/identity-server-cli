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
    public class IdentityResourceRepository : IIdentityResourceRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public IdentityResourceRepository(IConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(IdentityResource identityResource)
        {
            var identityResourceEntity = identityResource.ToEntity();

            await this._configurationDbContext.IdentityResources.AddAsync(identityResourceEntity);

            await this._configurationDbContext.SaveChangesAsync();
        }

        public async Task<IList<IdentityResource>> GetIdentityResourcesAsync()
            => await this._configurationDbContext.IdentityResources
                .OrderBy(i => i.Name)
                .Select(i => i.ToModel())
                .ToListAsync();
    }
}