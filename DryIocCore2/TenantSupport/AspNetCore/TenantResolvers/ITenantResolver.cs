using Microsoft.AspNetCore.Http;

namespace DryIocCore2.TenantSupport.AspNetCore.TenantResolvers
{
    /// <summary>
    /// Resolves tenant from http context
    /// </summary>
    public interface ITenantResolver
    {
        /// <summary>
        /// Resolves tenant from http context
        /// </summary>
        /// <param name="httpContext">The http context</param>
        /// <returns>Tenant string</returns>
        string ResolveTenant(HttpContext httpContext);
    }
}