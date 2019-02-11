using IdentityServerCli.Console.Extensions;
using Xunit;

namespace IdentityServerCli.Console.Test.Extensions
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("HelloWorld", "hello-world")]
        [InlineData("", "")]
        public void ShouldDashrializeAStringProperly(string str, string expected)
        {
            Assert.Equal(expected, str.Dashrialize());
        }
    }
}