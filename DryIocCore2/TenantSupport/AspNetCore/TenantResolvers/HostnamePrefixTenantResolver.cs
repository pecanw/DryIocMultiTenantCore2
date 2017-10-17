using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    public class HostnamePrefixTenantResolver : ITenantResolver
    {
        public string ResolveTenant(HttpContext httpContext)
        {
            string host = httpContext.Request.Host.Host;
            string tenant = null;

            // todo: Change to a more spohisticated algorithm enabling longer tenant names (currently just 2 chars are accepted)
            if (host != null && host.Length >= 3)
            {
                if (host[2] == '.' || host[2] == '-')
                {
                    tenant = host.Substring(0, 2).ToLower();
                }
            }
            return tenant;
        }
    }
}