using System;

namespace Umbraco.Docs.Preview.App.Interceptors.Caching
{

    [AttributeUsage(AttributeTargets.Method)]
    public class CacheIndefinitely : Attribute
    {
        public string CacheKey { get; set; }
    }
}
