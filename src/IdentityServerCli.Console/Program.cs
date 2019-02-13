using System;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServerCli.Console.Commands.ApiResources;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using IdentityServerCli.Console.Repositories;
using IdentityServerCli.Console.Interfaces.Repositories;

namespace IdentityServerCli.Console
{
    public static class Program
    {
        public const string ConnectionStringVariableName = "IS4_CONNECTION_STRING";

        public static int Main(string[] args) => BuildCommandLineApplication().Execute(args);

        public static CommandLineApplication BuildCommandLineApplication()
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
            });

            app.OnExecute(() =>
            {
                app.Out.WriteLine("Specify a subcommand");
                app.ShowHelp();
                return 1;
            });

            return app;
        }

        public static IServiceCollection GetServices()
        {
            IServiceCollection services = new ServiceCollection();

            var connectionString = Environment.GetEnvironmentVariable(ConnectionStringVariableName);

            services.AddSingleton<IConsole>(PhysicalConsole.Singleton);
            services.AddSingleton<IApiResourceRepository, ApiResourceRepository>();
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
