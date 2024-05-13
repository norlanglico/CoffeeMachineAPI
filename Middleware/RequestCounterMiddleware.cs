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
            // Check if it is April 1 and return status code 418 when true
            DateTime currentDate = DateTime.Now;
            if (currentDate.Month == 4 && currentDate.Day == 1)
            {
                context.Response.StatusCode = 418;
                context.Response.ContentLength = 0;
                return;
            }
            
            // Count request and store it in cache
            string cacheKey = "RequestCount";
            string? value = await _cache.GetStringAsync(cacheKey);
            int count = string.IsNullOrEmpty(value) ? 1 : int.Parse(value) + 1;
            await _cache.SetStringAsync(cacheKey, count.ToString());

            // Check the number of request and return status code 503 every fifth request
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