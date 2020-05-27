using System;

namespace Razorwing.Framework.Allocation
{
    public class ObjectUsage<T> : IDisposable
    {
        public T Object;
        public int Index;

        public long FrameId;

        internal Action<ObjectUsage<T>, UsageType> Finish;

        public UsageType Usage;

        public void Dispose()
        {
            Finish?.Invoke(this, Usage);
        }
    }

    public enum UsageType
    {
        None,
        Read,
        Write
    }
}
