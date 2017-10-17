using System;
using System.Diagnostics;
using System.Threading;
using DryIocCore2.Models;

namespace DryIocCore2.Services
{
    [DebuggerDisplay(nameof(TenantService) + " {" + nameof(InstanceNumber) + "} with " + nameof(IDependentService) + " {" + nameof(DependentService) + "}")]
    public class TenantService : ITenantService
    {
        private static int _counter;

        public static int Counter => _counter;


        private readonly Func<IRequestScopedService> _requestServiceFactory;
        private readonly Func<ITransientService> _transientServiceFactory;

        public int InstanceNumber { get; }

        public TenantService(IDependentService dependentService, IGlobalService globalService, 
            Func<IRequestScopedService> requestServiceFactory, Func<ITransientService> transientServiceFactory)
        {
            DependentService = dependentService;
            _requestServiceFactory = requestServiceFactory;
            _transientServiceFactory = transientServiceFactory;
            GlobalService = globalService;
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public IDependentService DependentService { get; }

        public IGlobalService GlobalService { get; }

        public IRequestScopedService RequestService => _requestServiceFactory();

        public ITransientService TransientService => _transientServiceFactory();

        public TestModel DescribeServices()
        {
            return new TestModel
            {
                Global = GlobalService.ToString(),
                Tenant = this.ToString(),
                Dependent = DependentService.ToString(),
                //Request = RequestService.ToString(),
                Transient = TransientService.ToString()
            };
        }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }
    }
}
