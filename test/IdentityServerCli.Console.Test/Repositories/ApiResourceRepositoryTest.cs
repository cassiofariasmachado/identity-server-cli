using FakeItEasy;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServerCli.Console.Repositories;
using Xunit;
using Models = IdentityServer4.Models;
using Entities = IdentityServer4.EntityFramework.Entities;
using System.Threading;

namespace IdentityServerCli.Console.Test.Repositories
{
    public class ApiResourceRepositoryTest
    {
        [Fact]
        public async void ShouldAddAndSaveTheApiResourceInTheContext()
        {
            var apiResource = new Models.ApiResource();
            var context = A.Fake<IConfigurationDbContext>();
            var apiRepository = new ApiResourceRepository(context);

            await apiRepository.AddAsync(apiResource);

            A.CallTo(() => context.SaveChangesAsync())
                .MustHaveHappened();

            A.CallTo(() => context.ApiResources.AddAsync(
                    A<Entities.ApiResource>.Ignored,
                    A<CancellationToken>.Ignored))
                .MustHaveHappened();
        }
    }
}