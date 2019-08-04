using Convey.MessageBrokers;
using Microsoft.AspNetCore.Http;
using Pacco.Services.Vehicles.Application;

namespace Pacco.Services.Vehicles.Infrastructure.Contexts
{
    internal sealed class AppContextFactory : IAppContextFactory
    {
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppContextFactory(ICorrelationContextAccessor contextAccessor, IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public IAppContext Create()
        {
            if (!(_contextAccessor.CorrelationContext is null))
            {
                return new AppContext(_contextAccessor.CorrelationContext as CorrelationContext);
            }

            var context = _httpContextAccessor.GetCorrelationContext();
            
            return context is null ? AppContext.Empty : new AppContext(context);
        }
    }
}