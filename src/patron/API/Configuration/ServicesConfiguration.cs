using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandHandlers.Email;
using Commands.Email;
using Core.CQRS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace API.Configuration
{
    public static class ServicesConfiguration
    {
        private static IEnumerable<TypeInfo> allTypes;

        public static void ConfigureServices(this IServiceCollection services) {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.AddSwaggerGen(c =>{
                c.SwaggerDoc("v1", new Info { Title = "Patron API", Version = "v1" });
            });

            RegisterCommandHandlers(services);
            RegisterCommandAndQueryBus(services);
        }

        private static void RegisterCommandAndQueryBus(IServiceCollection services)
        {
            services.AddSingleton<CommandBus>((IServiceProvider s) => new CommandBus(s));
        }

        private static void RegisterCommandHandlers(IServiceCollection services)
        {
            // Make sure we are referencing commands and command handlers so reflection can pick up the types
            var handlerType = typeof(UpdateEmailCommandHandler);
            var commandHandler = typeof(ICommandHandler<>);

            foreach (var classType in GetAllTypes())
            {
                foreach (var i in from i in classType.GetInterfaces()
                                  where i.IsGenericType && i.GetGenericTypeDefinition() == commandHandler
                                  select i)
                {
                    Console.WriteLine(i.FullName);
                    services.AddSingleton(i, classType);
                }
            }

            // services.AddSingleton<ICommandHandler<UpdateEmailCommand>, UpdateEmailCommandHandler>();
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
    }
}