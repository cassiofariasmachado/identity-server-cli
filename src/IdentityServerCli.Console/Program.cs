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
using IdentityServerCli.Console.Commands.IdentityResources;
using IdentityServerCli.Console.Commands.Clients;
using IdentityServerCli.Console.Extensions;
using IdentityServerCli.Console.Commands;

namespace IdentityServerCli.Console
{
    public static class Program
    {
        private const string Name = "Identity Server CLI";

        private const string Description = "A command line interface for manage clients, api and identity resources Identity Server 4.";

        public const string ConnectionStringVariableName = "IS4_CONNECTION_STRING";

        public static int Main(string[] args) => BuildCommandLineApplication().Execute(args);

        public static CommandLineApplication BuildCommandLineApplication()
        {
            var app = new CommandLineApplication
            {
                Name = Name,
                Description = Description
            };

            app.Conventions.UseDefaultConventions()
                .UseConstructorInjection(GetServices().BuildServiceProvider());

            app.Command(CommandsConsts.NewCommandName, newCmd =>
            {
                newCmd.Description = CommandsConsts.NewCommandDescription;

                newCmd.Command(nameof(ApiResource).Dashrialize(), app.GetService<NewApiResourceCommand>().Execute);
                newCmd.Command(nameof(IdentityResource).Dashrialize(), app.GetService<NewIdentityResourceCommand>().Execute);
                newCmd.Command(nameof(Client).Dashrialize(), app.GetService<NewClientCommand>().Execute);

                newCmd.OnExecute(() => newCmd.ShowSubCommandHelp());
            });

            app.OnExecute(() =>
            {
                app.Out.WriteLine("Specify a subcommand.");
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
            services.AddSingleton<IIdentityResourceRepository, IdentityResourceRepository>();
            services.AddSingleton<IClientRepository, ClientRepository>();

            services.AddSingleton<NewApiResourceCommand>();
            services.AddSingleton<NewIdentityResourceCommand>();
            services.AddSingleton<NewClientCommand>();

            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                });

            return services;
        }
    }
}
