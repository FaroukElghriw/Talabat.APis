using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Serivecs;

namespace Talabat.APis.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
      
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse=await cacheService.GetCacheResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse) ) 
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result= contentResult;
                return;// to not exe the end point of action 
            } // if we have a cachedResponse
            var executedEndPointContext = await next();
            if(executedEndPointContext.Result is OkObjectResult okObjectResult )
            { 
                await cacheService.CacheResponseAsync(cacheKey,okObjectResult,TimeSpan.FromSeconds(_timeToLiveSeconds));
            }

        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path); // urlbase=> /api/products/
            foreach (var(key, Value) in  request.Query.OrderBy(x=>x.Key)) 
            {
                keyBuilder.Append($"|{key}-{Value}");
            }

            return keyBuilder.ToString();
        }
    }
}
