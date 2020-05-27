using System;

namespace Razorwing.Framework.Caching
{
    public static class StaticCached
    {
        internal static bool BypassCache = false;
    }

    public struct Cached<T>
    {
        private T value;

        public T Value
        {
            get
            {
                if (!isValid)
                    throw new InvalidOperationException($"May not query {nameof(Value)} of an invalid {nameof(Cached<T>)}.");
                return value;
            }

            set
            {
                this.value = value;
                isValid = true;
            }
        }

        private bool isValid;

        public bool IsValid => !StaticCached.BypassCache && isValid;

        public static implicit operator T(Cached<T> value) => value.Value;

        /// <summary>
        /// Invalidate the cache of this object.
        /// </summary>
        /// <returns>True if we invalidated from a valid state.</returns>
        public bool Invalidate()
        {
            if (isValid)
            {
                isValid = false;
                return true;
            }

            return false;
        }
    }

    public struct Cached
    {
        private bool isValid;

        public bool IsValid => !StaticCached.BypassCache && isValid;

        /// <summary>
        /// Invalidate the cache of this object.
        /// </summary>
        /// <returns>True if we invalidated from a valid state.</returns>
        public bool Invalidate()
        {
            if (isValid)
            {
                isValid = false;
                return true;
            }

            return false;
        }

        public void Validate()
        {
            isValid = true;
        }
    }
}
