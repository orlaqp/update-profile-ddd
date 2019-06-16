using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandHandlers.Email;
using Commands.Email;
using Core.CQRS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace API.Configuration
{
    public static class ServicesConfiguration
    {
        private static IEnumerable<TypeInfo> allTypes;

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
            
            foreach (var classType in GetAllTypes().Where(t => !t.IsAbstract && IsSubclassOfRawGeneric(commandHandler, t)))
            {
                var parent = classType.BaseType;
                var commandType = parent.GetGenericArguments().Where(t => t.IsSubclassOf(typeof(Command))).FirstOrDefault();
                
                services.AddSingleton(commandType, classType);
            }
        }

        private static IEnumerable<TypeInfo> GetAllTypes() {
            if (allTypes == null) {
                allTypes = Assembly
                    .GetEntryAssembly()
                    .GetReferencedAssemblies()
                    .Select(Assembly.Load)
                    .SelectMany(x => x.DefinedTypes);
            }

            return allTypes;
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
            while (toCheck != null && toCheck != typeof(object)) {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}