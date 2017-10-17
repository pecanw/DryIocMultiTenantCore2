using System.Threading.Tasks;
using DryIocCore2.TenantSupport.AspNetCore.TenantResolvers;
using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore
{
    public class TenantSupportMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantSupportMiddleware(RequestDelegate next)
        {
            _next = next;
        }

#pragma warning disable UseAsyncSuffix // Use Async suffix
        public async Task Invoke(HttpContext httpContext, ITenantResolver tenantResolver, ITenantProvider tenantProvider)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            var tenant = tenantResolver.ResolveTenant(httpContext);

            if (string.IsNullOrEmpty(tenant))
            {
                // Call the next middleware delegate in the pipeline 
                await _next.Invoke(httpContext);
            }
            else
            {
                using (tenantProvider.BeginScope(tenant))
                {
                    // Call the next middleware delegate in the pipeline 
                    await _next.Invoke(httpContext);
                }
            }
        }
    }
}