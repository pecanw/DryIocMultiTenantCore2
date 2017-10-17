using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    /// <summary>
    /// A tenant resolver to be used in single-tenant environment
    /// </summary>
    public class NoTenantResolver : ITenantResolver
    {
        /// <inheritdoc />
        /// <returns>null</returns>
        public string ResolveTenant(HttpContext httpContext)
        {
            return null;
        }
    }
}