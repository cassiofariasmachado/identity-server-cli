using IdentityServerCli.Console.Extensions;
using Xunit;

namespace IdentityServerCli.Console.Test.Extensions
{
    public class StringExtensionsTest
    {
        [Fact]
        public void ShouldDashrializeAStringProperly()
        {
            var aString = "HelloWorld";

            Assert.Equal("hello-world", aString.Dashrialize());
        }

        [Fact]
        public void ShouldDashrializeReturnEmptyIfStringIsEmpty()
        {
            var aString = string.Empty;

            Assert.Equal(string.Empty, aString.Dashrialize());
        }
    }
}