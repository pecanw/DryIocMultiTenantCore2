using System;
using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    public class FixedTenantResolver : ITenantResolver
    {
        private readonly string _tenant;

        public FixedTenantResolver(string tenant)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                throw new ArgumentException("Tenant must be not empty string", nameof(tenant));
            }

            _tenant = tenant;
        }

        public string ResolveTenant(HttpContext httpContext)
        {
            return _tenant;
        }
    }
}