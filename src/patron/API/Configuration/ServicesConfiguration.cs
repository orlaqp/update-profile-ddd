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

            foreach (var classType in repositories)
            {
                services.AddScoped(classType);
            }
        }

        private static void RegisterCommandAndQueryBus(IServiceCollection services)
        {
            services.AddSingleton<CommandBus>();
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

            foreach (var classType in commandHandlers)
            {
                var parentType = classType.BaseType;
                var commandType = parentType
                    .GetGenericArguments()
                    .Where(t => t.IsSubclassOf(typeof(Command)))
                    .FirstOrDefault();

                if (commandType == null) {
                    throw new InvalidOperationException("Command type not found");
                }
                
                services.AddSingleton(commandType, classType);
            }
        }
    }
}