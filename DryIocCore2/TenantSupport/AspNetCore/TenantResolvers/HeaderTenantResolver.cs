using System;
using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    public class HeaderTenantResolver : ITenantResolver
    {
        public const string HeaderKey = "tenant";

        public string ResolveTenant(HttpContext httpContext)
        {
            var tenantHeaders = httpContext.Request.Headers[HeaderKey];
            switch (tenantHeaders.Count)
            {
                case 0:
                    return null;
                case 1:
                    return tenantHeaders[0];
                default:
                    throw new InvalidOperationException($"Multiple tenants specified in header {HeaderKey}: {tenantHeaders}");
            }
        }
    }
}