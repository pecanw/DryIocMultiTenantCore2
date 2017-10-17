using System.Diagnostics;
using System.Threading;

namespace DryIocCore2.Services
{
    [DebuggerDisplay(nameof(CustomDependentService) + " {" + nameof(InstanceNumber) + "}")]
    public class CustomDependentService : IDependentService
    {
        private static int _counter;

        public static int Counter => _counter;

        public int InstanceNumber { get; }

        public CustomDependentService()
        {
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }
    }
}