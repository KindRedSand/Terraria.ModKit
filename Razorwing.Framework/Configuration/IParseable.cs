namespace Razorwing.Framework.Configuration
{
    /// <summary>
    /// Represents a class which can be parsed from an arbitrary object.
    /// </summary>
    public interface IParseable
    {
        /// <summary>
        /// Parse an input into this instance.
        /// </summary>
        /// <param name="input">The input which is to be parsed.</param>
        void Parse(object input);
    }
}
