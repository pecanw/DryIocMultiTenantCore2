using System;
using System.Collections.Generic;
using DryIocCore2.Models;
using DryIocCore2.Services;
using DryIocCore2.TenantSupport;
using Microsoft.AspNetCore.Mvc;

namespace DryIocCore2.Controllers
{
    [Route("api/[controller]")]
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;
        private readonly ITenantProvider _tenantProvider;
        private readonly IMultiTenantService _multiTenantService;

        public TenantController(ITenantService tenantService, ITenantProvider tenantProvider, IMultiTenantService multiTenantService)
        {
            _tenantService = tenantService;
            _tenantProvider = tenantProvider;
            _multiTenantService = multiTenantService;
        }

        // GET api/tenant
        [HttpGet]
        public IEnumerable<TenantTestModel> Get()
        {
            var currentTenant = _tenantProvider.CurrentTenant;

            var res = new List<TenantTestModel>
            {
                new TenantTestModel(currentTenant, _tenantService.DescribeServices()),
                new TenantTestModel(currentTenant, _tenantService.DescribeServices())
            };

            return res;
        }

        // GET api/tenant/multiple
        [Route("multiple")]
        [HttpGet]
        public IEnumerable<TenantTestModel> MultipleTenants()
        {
            var currentTenant = _tenantProvider.CurrentTenant;

            var res = new List<TenantTestModel>();

            res.Add(new TenantTestModel(currentTenant, _tenantService.DescribeServices()));
            res.AddRange(_multiTenantService.GetServicesForTenants());

            return res;
        }

        // GET api/tenant/multiple2
        [Route("multiple2")]
        [HttpGet]
        public IEnumerable<TenantTestModel> MultipleTenants2()
        {
            var currentTenant = _tenantProvider.CurrentTenant;

            var res = new List<TenantTestModel>();

            res.Add(new TenantTestModel(currentTenant, _tenantService.DescribeServices()));
            res.AddRange(_multiTenantService.GetServicesForTenantsFromContainers());

            return res;
        }

    }
}
