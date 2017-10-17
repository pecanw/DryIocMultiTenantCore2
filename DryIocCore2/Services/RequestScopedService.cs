using System.Threading;

namespace DryIocCore2.Services
{
    public class RequestScopedService : IRequestScopedService
    {
        private static int _counter;

        public static int Counter => _counter;

        public int InstanceNumber { get; }

        public RequestScopedService()
        {
            InstanceNumber = Interlocked.Increment(ref _counter);
        }

        public override string ToString()
        {
            return $"{GetType().Name} {InstanceNumber}";
        }
    }
}