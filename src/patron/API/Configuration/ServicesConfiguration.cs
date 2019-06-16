using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandHandlers.Email;
using Commands.Email;
using Core.CQRS;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace API.Configuration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services) {
            services.AddSingleton<IServiceProvider>(serviceProvider => serviceProvider);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new Info { Title = "Patron API", Version = "v1" });
            });

            services.AddSingleton<ILogger>(serviceProvider => Log.Logger);
            

            RegisterCommandHandlers(services);
            RegisterCommandAndQueryBus(services);
        }

        private static void RegisterCommandAndQueryBus(IServiceCollection services)
        {
            services.AddSingleton<CommandBus>();
            services.AddSingleton<QueryBus>();
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
                var parent = classType.BaseType;
                var commandType = parent.GetGenericArguments().Where(t => t.IsSubclassOf(typeof(Command))).FirstOrDefault();
                
                services.AddSingleton(commandType, classType);
            }
        }
    }
}