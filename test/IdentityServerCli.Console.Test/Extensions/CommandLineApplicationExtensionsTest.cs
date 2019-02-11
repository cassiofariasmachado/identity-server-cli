using FakeItEasy;
using IdentityServerCli.Console.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Extensions
{
    public class CommandLineApplicationExtensionsTest
    {

        [Theory]
        [InlineData("help", "A help command.", CommandOptionType.SingleOrNoValue)]
        [InlineData("run", "Execute command.", CommandOptionType.NoValue)]
        public void ShouldCreateOptionProperly(
            string optionName,
            string description,
            CommandOptionType optionType
        )
        {
            var command = new CommandLineApplication();

            var option = command.CreateOption(optionName, description, optionType);

            Assert.Equal(optionName, option.LongName);
            Assert.Equal(description, option.Description);
            Assert.Equal(optionType, option.OptionType);
        }
    }
}