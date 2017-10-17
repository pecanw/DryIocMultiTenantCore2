using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    public class FallbackTenantResolver : ITenantResolver
    {
        private readonly IList<ITenantResolver> _resolvers;

        public FallbackTenantResolver(ITenantResolver resolver, string fallbackTenant)
        {
            _resolvers = new[] { resolver, new FixedTenantResolver(fallbackTenant) };
        }

        public FallbackTenantResolver(ITenantResolver resolver1, ITenantResolver resolver2, string fallbackTenant)
        {
            _resolvers = new[] { resolver1, resolver2, new FixedTenantResolver(fallbackTenant) };
        }

        public FallbackTenantResolver(IEnumerable<ITenantResolver> resolvers, string fallbackTenant)
        {
            _resolvers = resolvers.ToList();
            _resolvers.Add(new FixedTenantResolver(fallbackTenant));
        }

        public FallbackTenantResolver(params ITenantResolver[] resolvers)
        {
            _resolvers = resolvers;
        }

        public string ResolveTenant(HttpContext httpContext)
        {
            foreach (var resolver in _resolvers)
            {
                var tenant = resolver.ResolveTenant(httpContext);
                if (!string.IsNullOrEmpty(tenant))
                {
                    return tenant;
                }
            }
            return null;
        }
    }
}