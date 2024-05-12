using Microsoft.Extensions.Caching.Distributed;

namespace CoffeeMachineAPI.Middleware
{
    public class RequestCounterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RequestCounterMiddleware(RequestDelegate next, IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {                        
            string cacheKey = "RequestCount";
            string value = await _cache.GetStringAsync(cacheKey);
            int count = string.IsNullOrEmpty(value) ? 1 : int.Parse(value) + 1;

            await _cache.SetStringAsync(cacheKey, count.ToString());

            if (count % 5 == 0)
            {
                context.Response.StatusCode = 503;
                context.Response.ContentLength = 0;
                return;
            }

            await _next(context);
        }
    }
}