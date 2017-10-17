using System.Collections.Generic;
using DryIocCore2.Models;

namespace DryIocCore2.Services
{
    public interface IMultiTenantService
    {
        IEnumerable<TenantTestModel> GetServicesForTenants();

        IEnumerable<TenantTestModel> GetServicesForTenantsFromContainers();
    }
}