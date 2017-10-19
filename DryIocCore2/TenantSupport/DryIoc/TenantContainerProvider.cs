using System.Collections.Concurrent;
using DryIoc;

namespace DryIocCore2.TenantSupport.DryIoc
{
    public class TenantContainerProvider : ITenantContainerProvider
    {
        private readonly IContainer _rootContainer;

        private readonly ConcurrentDictionary<string, IContainer> _tenantContainers;

        private readonly ITenantProvider _tenantProvider;

        public TenantContainerProvider(IContainer rootContainer, ITenantProvider tenantProvider)
        {
            _rootContainer = rootContainer;
            _tenantProvider = tenantProvider;
            _tenantContainers = new ConcurrentDictionary<string, IContainer>();
        }

        public IContainer GetTenantContainer(string tenant)
        {
            if (string.IsNullOrEmpty(tenant))
            {
                return _rootContainer;
            }
            var tenantContainer = _tenantContainers.GetOrAdd(tenant, CreateTeantContainer);
            return tenantContainer;
        }

        public IContainer GetCurrentTenantContainer()
        {
            var tenant = _tenantProvider.CurrentTenant;
            return GetTenantContainer(tenant);
        }

        protected IContainer CreateTeantContainer(string tenant)
        {
            var teantContainer = _rootContainer
                .With(rules => rules.WithFactorySelector(Rules.SelectKeyedOverDefaultFactory(tenant)))
                .OpenScope(TenantReuse.TenantScopeName)
                .Resolve<IContainer>();

            return teantContainer;
        }

    }
}