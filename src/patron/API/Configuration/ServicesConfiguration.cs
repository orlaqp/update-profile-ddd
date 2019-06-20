using System;
using System.Linq;
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
using CommandHandlers.Email;
using Core.Domain;
using Domain.Patron.Events;
using Core.Audit;

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
            services.AddScoped<IAuditor, Auditor>();

            RegisterRepositories(services);
            RegisterCommandHandlers(services);
            RegisterDomainEventHandlers(services);
            RegisterBus(services);
        }

        private static void RegisterDomainEventHandlers(IServiceCollection services)
        {
            // Make sure we are referencing commands and command handlers so reflection can pick up the types
            var handlerType = typeof(PatronEmailUpdatedDomainEventHandler);
            var eventHandlers = ReflectionHelpers.GetSubClassesOf(typeof(DomainEventHandler<>));

            foreach (var eventHandler in eventHandlers)
            {
                var parentType = eventHandler.BaseType;
                var eventType = ReflectionHelpers.GetGenericArgument(
                    typeof(IDomainEvent),
                    parentType);
                
                if (eventType == null) {
                    throw new InvalidOperationException("Domain Event type not found");
                }
                
                services.AddScoped(eventType, eventHandler);
            }
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            var repositories = ReflectionHelpers.GetSubClassesOf(typeof(Repository<>));
            
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

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            // Make sure we are referencing commands and command handlers so reflection can pick up the types
            var handlerType = typeof(UpdateEmailCommandHandler);
            var commandHandlers = ReflectionHelpers.GetSubClassesOf(typeof(CommandHandler<>));
            
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

        private static void RegisterBus(IServiceCollection services)
        {
            services.AddScoped<ICommandBus, CommandBus>();
            services.AddScoped<IDomainEventBus, DomainEventBus>();
            // services.AddSingleton<QueryBus>();
        }
    }
}