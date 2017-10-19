using DryIoc;

namespace DryIocCore2.TenantSupport.DryIoc
{
    public static class TenantReuse
    {
        /// <summary>Special scope name that is recognized by <see cref="TenantSingleton"/> reuse.</summary>
        public static readonly string TenantScopeName = "TenantScope";

        /// <summary>Tenant context is just convention for reuse in <see cref="F:DryIoc.Reuse.InCurrentNamedScope(System.Object)" /> with special name <see cref="TenantScopeName"/>.</summary>
        public static readonly IReuse TenantSingleton = Reuse.InCurrentNamedScope(TenantScopeName);
    }
}