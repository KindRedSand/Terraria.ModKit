using System;
using System.Collections.Generic;

namespace Razorwing.Framework.Lists
{
    /// <summary>
    /// An IComparer that uses the result of a Comparison.
    /// </summary>
    public class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> comparison;

        public ComparisonComparer(Func<T, T, int> compare)
        {
            if (compare == null)
            {
                throw new ArgumentNullException(nameof(compare));
            }
            comparison = new Comparison<T>(compare);
        }

        public int Compare(T x, T y) => comparison(x, y);
    }
}
