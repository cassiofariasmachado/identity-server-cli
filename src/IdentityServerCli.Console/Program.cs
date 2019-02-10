using System;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.ApiResources;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IdentityServerCli.Console.Repositories;
using static System.Console;

namespace IdentityServerCli.Console
{
    class Program
    {
        private const string ConnectionStringVariableName = "IS4_CONNECTION_STRING";

        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "identity-server",
                Description = "A command line interface for Identity Serve 4"
            };

            app.Conventions.UseDefaultConventions()
                .UseConstructorInjection(GetServices().BuildServiceProvider());

            app.HelpOption(inherited: true);

            app.Command("new", newCmd =>
            {
                newCmd.Command("api-resource", app.GetService<NewApiResourceCommand>().Execute);

                string notImplemented = "Command not implemented yet";

                newCmd.Command("identity-resource", apiResourceCmd =>
                {
                    apiResourceCmd.OnExecute(() =>
                    {
                        // To-do: implement this command
                        WriteLine(notImplemented);
                    });
                });

                newCmd.Command("client", clientCmd =>
                {
                    clientCmd.OnExecute(() =>
                    {
                        // To-do: implement this command
                        WriteLine(notImplemented);
                    });
                });
            });

            app.OnExecute(() =>
            {
                WriteLine("Specify a subcommand");
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }

        public static IServiceCollection GetServices()
        {
            IServiceCollection services = new ServiceCollection();

            var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariableName);

            services.AddSingleton<IConsole>(PhysicalConsole.Singleton);
            services.AddSingleton<ApiResourceRepository>();
            services.AddSingleton<NewApiResourceCommand>();

            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                });

            return services;

        }
    }
}
