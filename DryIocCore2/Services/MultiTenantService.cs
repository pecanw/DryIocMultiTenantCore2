﻿using System;
using System.Collections.Generic;
using DryIoc;
using DryIocCore2.Models;
using DryIocCore2.TenantSupport;
using DryIocCore2.TenantSupport.DryIoc;

namespace DryIocCore2.Services
{
    public class MultiTenantService : IMultiTenantService
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly Func<ITenantService> _tenantServiceResolver;
        private readonly ITenantContainerProvider _tenantContainerProvider;

        public MultiTenantService(ITenantProvider tenantProvider, Func<ITenantService> tenantServiceResolver, ITenantContainerProvider tenantContainerProvider)
        {
            _tenantProvider = tenantProvider;
            _tenantServiceResolver = tenantServiceResolver;
            _tenantContainerProvider = tenantContainerProvider;
        }

        public IEnumerable<TenantTestModel> GetServicesForTenants()
        {
            for (int i = 1; i <= 3; i++)
            {
                string tenant = $"t{i}";

                using (_tenantProvider.BeginScope(tenant))
                {
                    // Here I want to get the service for specific tenant
                    var tenantService = _tenantServiceResolver();
                    yield return new TenantTestModel(tenant, tenantService.DescribeServices());
                }
            }
        }

        public IEnumerable<TenantTestModel> GetServicesForTenantsFromContainers()
        {
            for (int i = 1; i <= 3; i++)
            {
                string tenant = $"t{i}";

                using (_tenantProvider.BeginScope(tenant))
                {
                    // working but ugly (I don't want to flood the services code with the "tenant-related services / code")
                    var tenantContainer =  _tenantContainerProvider.GetTenantContainer(tenant);
                    var tenantService = tenantContainer.Resolve<ITenantService>();
                    yield return new TenantTestModel(tenant, tenantService.DescribeServices());
                }
            }
        }

    }
}