using System;
using FakeItEasy;
using IdentityServerCli.Console.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Extensions
{
    public class IConsoleExtensionsTest
    {
        [Theory]
        [InlineData("Command completed successfully.")]
        [InlineData("This is a success message.")]
        public void ShouldWriteSuccessProperly(string text)
        {
            var console = A.Fake<IConsole>();

            console.WriteSuccess(text);

            A.CallTo(() => console.Out.WriteLine(text))
                .MustHaveHappened();

            A.CallToSet(() => console.ForegroundColor)
                .To(ConsoleColor.Green)
                .MustHaveHappened();

            A.CallTo(() => console.ResetColor())
                .MustHaveHappened();
        }

        [Theory]
        [InlineData("Mayday, Mayday, Mayday.")]
        [InlineData("Something went wrong, please contact the system administrator.")]
        public void ShouldWriteErrorProperly(string text)
        {
            var console = A.Fake<IConsole>();

            console.WriteError(text);

            A.CallTo(() => console.Error.WriteLine(text))
                .MustHaveHappened();

            A.CallToSet(() => console.ForegroundColor)
                .To(ConsoleColor.Red)
                .MustHaveHappened();

            A.CallTo(() => console.ResetColor())
                .MustHaveHappened();
        }
    }
}