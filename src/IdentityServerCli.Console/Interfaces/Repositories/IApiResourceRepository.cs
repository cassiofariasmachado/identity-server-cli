using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServerCli.Console.Interfaces.Repositories
{
    public interface IApiResourceRepository
    {
        Task AddAsync(ApiResource apiResource);
    }
}