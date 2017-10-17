using System;

namespace DryIocCore2.TenantSupport
{
    /// <summary>
    /// Support for async-aware tenant context to be used in a DI container
    /// </summary>
    public interface ITenantProvider
    {
        /// <summary>
        /// Get current tenant context
        /// </summary>
        string CurrentTenant { get; }

        /// <summary>
        /// Creates a new tenant scope and returns IDisposable to be disposed at the end of the scope
        /// </summary>
        /// <param name="tenant">A string Tenant id</param>
        /// <returns>Returns IDisposable to be disposed at the end of the scope</returns>
        IDisposable BeginScope(string tenant);
    }
}