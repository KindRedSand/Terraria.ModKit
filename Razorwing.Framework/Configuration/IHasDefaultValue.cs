namespace Razorwing.Framework.Configuration
{
    /// <summary>
    /// Interface for objects that have a default value.
    /// </summary>
    public interface IHasDefaultValue
    {
        /// <summary>
        /// Check whether this object has its default value.
        /// </summary>
        bool IsDefault { get; }
    }
}
