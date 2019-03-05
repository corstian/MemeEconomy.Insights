using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MemeEconomy.Insights
{
    public class ContextProvider<T> : IContextProvider<T>
    {
        readonly IHttpContextAccessor _contextAccessor;
        public ContextProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public T Get()
        {
            return _contextAccessor?.HttpContext?.RequestServices == null
                ? default(T)
                : _contextAccessor.HttpContext.RequestServices.GetService<T>();
        }
    }
}
