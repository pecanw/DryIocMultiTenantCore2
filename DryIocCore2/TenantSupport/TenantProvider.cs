using System;
using System.Diagnostics;

namespace DryIocCore2.TenantSupport
{
    /// <inheritdoc />
    [DebuggerDisplay("{" + nameof(CurrentTenant) + "}")]
    public class TenantProvider : ITenantProvider
    {
        /// <inheritdoc />
        public string CurrentTenant => TenantContext.CurrentTenant;

        /// <inheritdoc />
        public IDisposable BeginScope(string tenant)
        {
            return TenantContext.BeginScope(tenant);
        }

        public override string ToString()
        {
            return CurrentTenant;
        }
    }
}