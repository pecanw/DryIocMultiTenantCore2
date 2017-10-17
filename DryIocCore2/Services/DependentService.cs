using System.Diagnostics;
using System.Threading;

namespace DryIocCore2.Services
{
    [DebuggerDisplay(nameof(DependentService) + " {" + nameof(InstanceNumber) + "}")]
    public class DependentService : IDependentService
    {
        private static int _counter;

        public static int Counter => _counter;

        public int InstanceNumber { get; }

        public DependentService()
        {
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }
    }
}
