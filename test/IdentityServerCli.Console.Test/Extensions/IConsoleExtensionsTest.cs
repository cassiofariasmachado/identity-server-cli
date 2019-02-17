using System;
using FakeItEasy;
using IdentityServerCli.Console.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test.Extensions
{
    public class IConsoleExtensionsTest
    {
        private readonly IConsole _console;

        public IConsoleExtensionsTest()
        {
            _console = A.Fake<IConsole>();
        }

        [Theory]
        [InlineData("Command completed successfully.")]
        [InlineData("This is a success message.")]
        public void ShouldWriteSuccessProperly(string text)
        {
            _console.WriteSuccess(text);

            WriteLineMustHaveHappend(text);
            ShouldHaveChangedTheColor(ConsoleColor.Green);
            ResetColorMustHaveHappend();
        }

        [Theory]
        [InlineData("Mayday, Mayday, Mayday.")]
        [InlineData("Something went wrong, please contact the system administrator.")]
        public void ShouldWriteErrorProperly(string text)
        {
            _console.WriteError(text);

            WriteErrorLineMustHaveHappend(text);
            ShouldHaveChangedTheColor(ConsoleColor.Red);
            ResetColorMustHaveHappend();
        }

        [Theory]
        [InlineData("It's just a warning.")]
        [InlineData("Please, I'm a warning, don't ignore me.")]
        public void ShouldWriteWarningProperly(string text)
        {
            _console.WriteWarning(text);

            WriteLineMustHaveHappend(text);
            ShouldHaveChangedTheColor(ConsoleColor.Yellow);
            ResetColorMustHaveHappend();
        }

        private void ResetColorMustHaveHappend() =>
            A.CallTo(() => _console.ResetColor())
                .MustHaveHappened();

        private void ShouldHaveChangedTheColor(ConsoleColor color) =>
            A.CallToSet(() => _console.ForegroundColor)
                .To(color)
                .MustHaveHappened();

        private void WriteLineMustHaveHappend(string text) =>
            A.CallTo(() => _console.Out.WriteLine(text))
                .MustHaveHappened();

        private void WriteErrorLineMustHaveHappend(string text) =>
            A.CallTo(() => _console.Error.WriteLine(text))
                .MustHaveHappened();
    }
}