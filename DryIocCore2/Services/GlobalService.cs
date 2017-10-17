using System.Diagnostics;
using System.Threading;

namespace DryIocCore2.Services
{
    [DebuggerDisplay(nameof(GlobalService) + " {" + nameof(InstanceNumber) + "}")]
    public class GlobalService : IGlobalService
    {
        private static int _counter;

        public GlobalService()
        {
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public static int Counter => _counter;

        public int InstanceNumber { get; }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }

    }
}
