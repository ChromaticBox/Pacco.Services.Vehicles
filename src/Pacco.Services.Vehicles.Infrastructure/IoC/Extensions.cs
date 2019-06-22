using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Convey;
using Convey.CQRS.Commands;
using Microsoft.Extensions.DependencyInjection;
using Pacco.Services.Vehicles.Application.Commands;
using Scrutor;

namespace Pacco.Services.Vehicles.Infrastructure.IoC
{
    internal static class Extensions
    {
        public static IConveyBuilder RegisterExceptionDecorators(this IConveyBuilder builder)
        {
            var commandHandlers = typeof(AddVehicle).Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
                .ToList();

            commandHandlers.ForEach(ch => GetExtensionMethods()
                .FirstOrDefault(mi => !mi.IsGenericMethod && mi.Name == "Decorate")?
                .Invoke(builder.Services, new object[]
                {
                    builder.Services, 
                    ch.GetInterfaces().FirstOrDefault(), 
                    typeof(CommandHandlerExceptionDecorator<>).
                        MakeGenericType(ch.GetInterfaces().FirstOrDefault()?.GenericTypeArguments.First())
                }));

            return builder;
        }
        
        private static IEnumerable<MethodInfo> GetExtensionMethods()
        {
            var types = typeof(ReplacementBehavior).Assembly.GetTypes();

            var query = from type in types
                where type.IsSealed && !type.IsGenericType && !type.IsNested
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                where method.IsDefined(typeof(ExtensionAttribute), false)
                where method.GetParameters()[0].ParameterType == typeof(IServiceCollection)
                select method;
            return query;
        }
    }
}