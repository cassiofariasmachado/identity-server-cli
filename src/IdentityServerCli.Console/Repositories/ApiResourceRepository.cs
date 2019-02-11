using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;

namespace IdentityServerCli.Console.Repositories
{
    public class ApiResourceRepository
    {
        private readonly IConfigurationDbContext _configurationDbContext;

        public ApiResourceRepository(IConfigurationDbContext configurationDbContext)
        {
            _configurationDbContext = configurationDbContext;
        }

        public async Task AddAsync(ApiResource apiResource)
        {
            var apiResourceEntity = apiResource.ToEntity();

            await _configurationDbContext.ApiResources.AddAsync(apiResourceEntity);

            await _configurationDbContext.SaveChangesAsync();
        }
    }
}