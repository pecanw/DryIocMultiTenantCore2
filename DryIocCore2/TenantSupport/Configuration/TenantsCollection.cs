using System;
using System.Collections;
using System.Collections.Generic;

namespace DryIocCore2.TenantSupport.Configuration
{
    public class TenantsCollection<T> : IDictionary<string, T>, IReadOnlyCollection<T>
        where T : TenantConfigBase
    {
        private readonly IDictionary<string, T> _tenants = new Dictionary<string, T>(StringComparer.InvariantCultureIgnoreCase);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            yield break;
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _tenants.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _tenants).GetEnumerator();
        }

        public void Add(KeyValuePair<string, T> item)
        {
            EnsureTenantNameMatchesKey(item.Key, item.Value);
            _tenants.Add(item);
        }

        public void Clear()
        {
            _tenants.Clear();
        }

        public bool Contains(KeyValuePair<string, T> item)
        {
            return _tenants.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            _tenants.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, T> item)
        {
            return _tenants.Remove(item);
        }

        public int Count => _tenants.Count;

        public bool IsReadOnly => _tenants.IsReadOnly;

        public void Add(string tenant, T tenantConfig)
        {
            EnsureTenantNameMatchesKey(tenant, tenantConfig);
            _tenants.Add(tenant, tenantConfig);
        }

        public bool ContainsKey(string tenant)
        {
            return _tenants.ContainsKey(tenant);
        }

        public bool Remove(string tenant)
        {
            return _tenants.Remove(tenant);
        }

        public bool TryGetValue(string tenant, out T tenantConfig)
        {
            return _tenants.TryGetValue(tenant, out tenantConfig);
        }

        public T this[string tenant]
        {
            get
            {
                if (!_tenants.ContainsKey(tenant))
                {
                    throw new ArgumentOutOfRangeException(nameof(tenant), $"Tenant configuration for '{tenant}' not found.");
                }
                return _tenants[tenant];
            }
            set => _tenants[tenant] = value;
        }

        public ICollection<string> Keys => _tenants.Keys;

        public ICollection<T> Values => _tenants.Values;

        private static void EnsureTenantNameMatchesKey(string tenant, T tenantConfig)
        {
            if (tenantConfig.Name != null && tenantConfig.Name != tenant)
            {
                throw new InvalidOperationException("The tenant name doesn't match the tenant");
            }
            if (tenantConfig.Name == null)
            {
                tenantConfig.Name = tenant;
            }
        }
    }
}