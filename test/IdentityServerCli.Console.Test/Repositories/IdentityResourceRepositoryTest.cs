using FakeItEasy;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServerCli.Console.Repositories;
using Xunit;
using Models = IdentityServer4.Models;
using Entities = IdentityServer4.EntityFramework.Entities;
using System.Threading;

namespace IdentityServerCli.Console.Test.Repositories
{
    public class IdentityResourceRepositoryTest
    {
        [Fact]
        public async void ShouldAddAndSaveTheIdentityResourceInTheContext()
        {
            var identityResource = new Models.IdentityResource();
            var context = A.Fake<IConfigurationDbContext>();
            var identityResourceRepository = new IdentityResourceRepository(context);

            await identityResourceRepository.AddAsync(identityResource);

            A.CallTo(() => context.SaveChangesAsync())
                .MustHaveHappened();

            A.CallTo(() => context.IdentityResources.AddAsync(
                    A<Entities.IdentityResource>.Ignored,
                    A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }
    }
}