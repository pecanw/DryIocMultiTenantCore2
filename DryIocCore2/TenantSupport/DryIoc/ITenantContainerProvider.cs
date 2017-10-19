using DryIoc;

namespace DryIocCore2.TenantSupport.DryIoc
{
    public interface ITenantContainerProvider
    {
        IContainer GetTenantContainer(string tenant);

        IContainer GetCurrentTenantContainer();
    }
}