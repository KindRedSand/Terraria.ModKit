using System.Threading;

namespace Razorwing.Framework.Threading
{
    public class AtomicCounter
    {
        private long count;

        public long Increment()
        {
            return Interlocked.Increment(ref count);
        }

        public long Add(long value)
        {
            return Interlocked.Add(ref count, value);
        }

        public long Reset()
        {
            return Interlocked.Exchange(ref count, 0);
        }

        public long Value
        {
            set => Interlocked.Exchange(ref count, value);
            get => Interlocked.Read(ref count);
        }
    }
}
