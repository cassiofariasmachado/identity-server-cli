using System.Threading.Tasks;
using IdentityServer4.Models;

namespace IdentityServerCli.Console.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task AddAsync(Client client);
    }
}