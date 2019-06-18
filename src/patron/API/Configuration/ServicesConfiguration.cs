using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandHandlers.Email;
using Commands.Email;
using Core.CQRS;
using Core.Helpers;
using Infrastructure.SQLServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Infrastructure.SQLServer.Repositories;
using Core.Domain;
using Infrastructure.Repositories;

namespace API.Configuration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config) {
            services.AddSingleton<IServiceProvider>(serviceProvider => serviceProvider);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new Info { Title = "Patron API", Version = "v1" });
            });

            services.AddSingleton<ILogger>(serviceProvider => Log.Logger);

            services.AddDbContext<CommandsDbContext>(
                options => options.UseSqlServer(config.GetConnectionString("PatronDB"))
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            RegisterRepositories(services);
            RegisterCommandHandlers(services);
            RegisterCommandAndQueryBus(services);
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            var repository = typeof(Repository<>);
            var repositories = ReflectionHelpers
                .GetAllTypes()
                .Where(t => !t.IsAbstract && ReflectionHelpers.IsSubclassOfRawGeneric(repository, t));

            foreach (var repositoryType in repositories)
            {
                var implementedInterfaces = repositoryType.ImplementedInterfaces;
                var repositoryInterface = repositoryType
                    .ImplementedInterfaces
                    .FirstOrDefault(x => x.Name.EndsWith("Repository"));

                if (repositoryInterface == null) {
                    throw new InvalidOperationException($"Class {repositoryType.FullName} inherit Repository but does not implement any interface that ends in 'Repository'");
                }

                services.AddScoped(repositoryInterface, repositoryType);
            }
        }

        private static void RegisterCommandAndQueryBus(IServiceCollection services)
        {
            services.AddScoped<CommandBus>();
            // services.AddSingleton<QueryBus>();
        }

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            // Make sure we are referencing commands and command handlers so reflection can pick up the types
            var handlerType = typeof(UpdateEmailCommandHandler);
            var commandHandler = typeof(CommandHandler<>);
            var commandHandlers = ReflectionHelpers
                .GetAllTypes()
                .Where(t => !t.IsAbstract && ReflectionHelpers.IsSubclassOfRawGeneric(commandHandler, t));

            foreach (var commandHandlerType in commandHandlers)
            {
                var parentType = commandHandlerType.BaseType;
                var commandType = parentType
                    .GetGenericArguments()
                    .Where(t => t.IsSubclassOf(typeof(Command)))
                    .FirstOrDefault();

                if (commandType == null) {
                    throw new InvalidOperationException("Command type not found");
                }
                
                services.AddScoped(commandType, commandHandlerType);
            }
        }
    }
}