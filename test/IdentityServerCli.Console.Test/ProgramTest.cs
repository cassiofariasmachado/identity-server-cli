using System;
using McMaster.Extensions.CommandLineUtils;
using Xunit;

namespace IdentityServerCli.Console.Test
{
    public class ProgramTest
    {
        [Fact]
        public void ShouldReturnOneWhenExecuteCommandLineApplicationWithNoArgs()
        {
            var app = CreateCommandLineApplication();

            var result = app.Execute();

            Assert.Equal(1, result);
        }

        public CommandLineApplication CreateCommandLineApplication()
        {
            Environment.SetEnvironmentVariable(
                Program.ConnectionStringVariableName,
                "FakeConnectionStringVariableName"
            );

            return Program.BuildCommandLineApplication();
        }
    }
}