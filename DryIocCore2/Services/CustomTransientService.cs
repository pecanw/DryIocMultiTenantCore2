using System.Diagnostics;
using System.Threading;

namespace DryIocCore2.Services
{
    [DebuggerDisplay(nameof(CustomTransientService) + " {" + nameof(InstanceNumber) + "} with " + nameof(IDependentService) + " {" + nameof(DependentService) + "}")]
    public class CustomTransientService : ITransientService
    {
        private static int _counter;

        public static int Counter => _counter;

        public int InstanceNumber { get; }

        public CustomTransientService(IDependentService dependentService)
        {
            DependentService = dependentService;
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public IDependentService DependentService { get; }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }
    }
}