using DryIoc;
using DryIocCore2.Services;

namespace DryIocCore2
{
    public class CompositionRoot
    {
        // If you need the whole container then change parameter type from IRegistrator to IContainer
        public CompositionRoot(IContainer container)
        {
            container.Register<TenantContainerManager>(Reuse.Singleton);

            container.Register<IMultiTenantService, MultiTenantService>(Reuse.Singleton);
            container.Register<IGlobalService, GlobalService>(Reuse.Singleton);

            // These registrations should be used as a "fallback" if no tenant is selected 
            container.Register<ITenantService, TenantService>(Reuse.Singleton);
            container.Register<IDependentService, DependentService>(Reuse.Singleton);

            container.Register<IRequestScopedService, RequestScopedService>(Reuse.InWebRequest);
            container.Register<ITransientService, TransientService>(Reuse.Transient);

            var r = container.Resolve<TenantContainerManager>();

            for (int i = 1; i <= 3; i++)
            {
                string tenant = $"t{i}";

                var tenantContainer = r.GetTenantContainer(tenant);

                //register tenant scoped services
                tenantContainer.Register<ITenantService, TenantService>(Reuse.InCurrentNamedScope(tenant), ifAlreadyRegistered: IfAlreadyRegistered.Replace);
                tenantContainer.Register<IDependentService, DependentService>(Reuse.InCurrentNamedScope(tenant), ifAlreadyRegistered: IfAlreadyRegistered.Replace);

                // custom service implementations
                switch (i)
                {
                    case 2:
                        // we want to use CustomDependentService in "t2"
                        container.Register<IDependentService, CustomDependentService>(Reuse.InCurrentNamedScope(tenant), serviceKey: tenant);
                        break;
                    case 3:
                        // we want to use CustomTransientService in "t3"
                        container.Register<ITransientService, CustomTransientService>(Reuse.Transient, serviceKey: tenant);
                        break;
                }
            }
        }
    }
}