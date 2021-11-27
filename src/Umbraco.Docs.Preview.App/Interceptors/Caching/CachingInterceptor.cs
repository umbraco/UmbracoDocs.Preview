using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Umbraco.Docs.Preview.App.Interceptors.Caching
{
    public class CachingInterceptor : IInterceptor
    {
        private readonly ILogger<CachingInterceptor> _log;
        private readonly IMemoryCache _cache;

        public CachingInterceptor(ILogger<CachingInterceptor> log, IMemoryCache cache)
        {
            _log = log;
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {
            var attribute = invocation.Method.GetCustomAttribute(typeof(CacheIndefinitely)) as CacheIndefinitely;
            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            if (_cache.TryGetValue(attribute.CacheKey, out var value))
            {
                _log.LogDebug("Cache hit for {info}", invocation.Method);
                invocation.ReturnValue = value;
                return;
            }

            _log.LogDebug("Cache miss for {info}", invocation.Method);
            invocation.Proceed();
            _cache.Set(attribute.CacheKey, invocation.ReturnValue);
        }
    }
}
