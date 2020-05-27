using System;

namespace Razorwing.Framework.IO.Stores
{
    public class ChangeableResourceStore<T> : ResourceStore<T>
    {
        public event Action<string> OnChanged;

        protected void TriggerOnChanged(string name)
        {
            OnChanged?.Invoke(name);
        }
    }
}
