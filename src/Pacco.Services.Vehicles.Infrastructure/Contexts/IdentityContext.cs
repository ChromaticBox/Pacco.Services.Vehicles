using Pacco.Services.Vehicles.Application;

namespace Pacco.Services.Vehicles.Infrastructure.Contexts
{
    public class IdentityContext : IIdentityContext
    {
        public string Id { get; }
        public string Role { get; }
        public bool IsAuthenticated { get; }

        internal IdentityContext()
        {
        }

        public IdentityContext(CorrelationContext.UserContext context)
            : this(context.Id, context.Role, context.IsAuthenticated)
        {
        }
        
        public IdentityContext(string id, string role, bool isAuthenticated)
        {
            Id = id;
            Role = role;
            IsAuthenticated = isAuthenticated;
        }
    }
}