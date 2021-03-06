using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServerCli.Console.Interfaces.Repositories
{
    public interface IIdentityResourceRepository
    {
        Task AddAsync(IdentityResource identityResource);

        Task<IList<IdentityResource>> GetIdentityResourcesAsync();
    }
}