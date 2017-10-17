using DryIocCore2.Models;

namespace DryIocCore2.Services
{
    public interface ITenantService
    {
        IDependentService DependentService { get; }
        int InstanceNumber { get; }
        IGlobalService GlobalService { get; }
        IRequestScopedService RequestService { get; }
        ITransientService TransientService { get; }
        TestModel DescribeServices();
    }
}