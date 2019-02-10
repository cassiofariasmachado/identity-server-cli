using McMaster.Extensions.CommandLineUtils;

namespace IdentityServerCli.Console.Commands
{
    public interface ICommand
    {
        void Execute(CommandLineApplication command);
    }
}