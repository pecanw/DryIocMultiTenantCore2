using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    public class TopLevelDomainTenantResolver : ITenantResolver
    {
        public string ResolveTenant(HttpContext httpContext)
        {
            string host = httpContext.Request.Host.Host;

            string tenant = null;

            var idx = host.LastIndexOf('.');
            if (idx >= 0 && idx < host.Length - 1)
            {
                tenant = host.Substring(idx + 1).ToLower();
            }
            return tenant;
        }
    }
}