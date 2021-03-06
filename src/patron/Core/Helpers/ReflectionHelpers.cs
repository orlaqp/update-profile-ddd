using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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

        public static IEnumerable<TypeInfo> GetSubClassesOf(Type type) {
            return ReflectionHelpers
                .GetAllTypes()
                .Where(t => !t.IsAbstract && ReflectionHelpers.IsSubclassOfRawGeneric(type, t));
        }

        public static Type GetGenericArgument(Type searchType, Type genericType) {
            return genericType
                .GetGenericArguments()
                .Where(t => t.GetInterfaces().Contains(searchType))
                .FirstOrDefault();
        }

        public static object GetPropertyValue(object obj, Type entityType, string propertyName)
        {
            return entityType
                .GetField(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(obj);
        }

        public static Task InvokeAsyncMethod(object obj, string methodName, object[] parameters) {
            var objType = obj.GetType();
            var handleMethod = objType.GetMethod(methodName);
            var taskResult = (Task)handleMethod.Invoke(obj, parameters);

            return taskResult;
        }
    }
}