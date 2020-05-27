namespace Razorwing.Framework.Configuration
{
    public interface IConfigManager
    {
        /// <summary>
        /// Loads this config.
        /// </summary>
        void Load();

        /// <summary>
        /// Saves this config.
        /// </summary>
        /// <returns>Whether the operation succeeded.</returns>
        bool Save();
    }
}
