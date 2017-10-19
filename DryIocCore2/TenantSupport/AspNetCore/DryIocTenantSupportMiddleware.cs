using System;
using System.Threading.Tasks;
using DryIoc;
using DryIocCore2.TenantSupport.AspNetCore.TenantResolvers;
using DryIocCore2.TenantSupport.DryIoc;
using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore
{
    public class DryIocTenantSupportMiddleware
    {
        private readonly RequestDelegate _next;

        public DryIocTenantSupportMiddleware(RequestDelegate next)
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
                var tenantContainerProvider = (ITenantContainerProvider) httpContext.RequestServices.GetService(typeof(ITenantContainerProvider));
                var tenantContainer = tenantContainerProvider.GetTenantContainer(tenant);
                using (var scopedConatiner = tenantContainer.OpenScope(Reuse.WebRequestScopeName))
                {
                    // replace the RequestScope container by the tenant request scope
                    httpContext.RequestServices = scopedConatiner.Resolve<IServiceProvider>();

                    // Call the next middleware delegate in the pipeline 
                    await _next.Invoke(httpContext);
                }
            }
        }
    }
}