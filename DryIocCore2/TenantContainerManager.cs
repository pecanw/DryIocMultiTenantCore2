using System.Collections.Concurrent;
using DryIoc;

namespace DryIocCore2
{
    public class TenantContainerManager
    {
        private IContainer _rootContainer;

        private ConcurrentDictionary<string, IContainer> _tenantContainers;

        public TenantContainerManager(IContainer rootContainer)
        {
            _rootContainer = rootContainer;
            _tenantContainers = new ConcurrentDictionary<string, IContainer>();
        }

        public IContainer GetTenantContainer(string tenant)
        {
            return _tenantContainers.GetOrAdd(tenant,
                t =>
                    _rootContainer
                        .With(rules => rules.WithFactorySelector(Rules.SelectKeyedOverDefaultFactory(t)))
                        .OpenScope(t)
                        .Resolve<IContainer>());
        }
    }
}