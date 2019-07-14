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
    public class ApiResourceRepository : IApiResourceRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public ApiResourceRepository(IConfigurationDbContext configurationDbContext)
        {
            this._configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(ApiResource apiResource)
        {
            var apiResourceEntity = apiResource.ToEntity();

            await this._configurationDbContext.ApiResources.AddAsync(apiResourceEntity);

            await this._configurationDbContext.SaveChangesAsync();
        }

        public async Task<IList<ApiResource>> GetApiResourcesAsync()
            => await this._configurationDbContext.ApiResources
                .OrderBy(a => a.Name)
                .Select(a => a.ToModel())
                .ToListAsync();
    }
}