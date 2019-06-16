using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Helpers
{
    public static class ReflectionHelpers
    {
        public static IEnumerable<TypeInfo> GetAllTypes() {
            var allTypes = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .SelectMany(x => x.DefinedTypes);
            
            return allTypes;
        }

        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
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