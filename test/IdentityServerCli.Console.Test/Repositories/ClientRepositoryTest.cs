using FakeItEasy;
using Xunit;
using Models = IdentityServer4.Models;
using Entities = IdentityServer4.EntityFramework.Entities;
using System.Threading;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServerCli.Console.Repositories;

namespace IdentityServerCli.Console.Test.Repositories
{
    public class ClientRepositoryTest
    {
        [Fact]
        public async void ShouldAddAndSaveTheClientInTheContext()
        {
            var client = new Models.Client();
            var context = A.Fake<IConfigurationDbContext>();
            var clientRepository = new ClientRepository(context);

            await clientRepository.AddAsync(client);

            A.CallTo(() => context.SaveChangesAsync())
                .MustHaveHappened();

            A.CallTo(() => context.Clients.AddAsync(
                    A<Entities.Client>.Ignored,
                    A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }
    }
}