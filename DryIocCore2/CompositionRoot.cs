using DryIoc;
using DryIocCore2.Services;
using DryIocCore2.TenantSupport.DryIoc;

namespace DryIocCore2
{
    public class CompositionRoot
    {
        // If you need the whole container then change parameter type from IRegistrator to IContainer
        public CompositionRoot(IContainer container)
        {
            container.Register<ITenantContainerProvider, TenantContainerProvider>(Reuse.Singleton);

            container.Register<IMultiTenantService, MultiTenantService>(Reuse.Singleton);
            container.Register<IGlobalService, GlobalService>(Reuse.Singleton);

            // tenant singleton services
            container.Register<ITenantService, TenantService>(TenantReuse.TenantSingleton); 
            container.Register<IDependentService, DependentService>(TenantReuse.TenantSingleton);

            container.Register<IRequestScopedService, RequestScopedService>(Reuse.Scoped);
            container.Register<ITransientService, TransientService>(Reuse.Transient);

            // custom tenant services

            // we want to use CustomDependentService in "t2"
            container.Register<IDependentService, CustomDependentService>(TenantReuse.TenantSingleton, serviceKey: "t2");

            // we want to use CustomTransientService in "t3"
            container.Register<ITransientService, CustomTransientService>(Reuse.Transient, serviceKey: "t3");

        }
    }
}