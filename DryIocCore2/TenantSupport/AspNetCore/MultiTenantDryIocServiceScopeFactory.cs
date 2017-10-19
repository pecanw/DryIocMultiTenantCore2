using System;
using DryIoc;
using DryIocCore2.TenantSupport.AspNetCore.TenantResolvers;
using DryIocCore2.TenantSupport.DryIoc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DryIocCore2.TenantSupport.AspNetCore
{
    public class MultiTenantDryIocServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ITenantContainerProvider _tenantContainerProvider;
        private readonly ITenantResolver _tenantResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultiTenantDryIocServiceScopeFactory(ITenantContainerProvider tenantContainerProvider, ITenantResolver tenantResolver, IHttpContextAccessor httpContextAccessor)
        {
            _tenantContainerProvider = tenantContainerProvider;
            _tenantResolver = tenantResolver;
            _httpContextAccessor = httpContextAccessor;
        }

        public IServiceScope CreateScope()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string tenant = null;
            if (httpContext != null)
            {
                tenant = _tenantResolver.ResolveTenant(httpContext);
            }
            var scopedConatiner = _tenantContainerProvider.GetTenantContainer(tenant).OpenScope(Reuse.WebRequestScopeName);
            return scopedConatiner.Resolve<IServiceScope>();
        }
    }
}