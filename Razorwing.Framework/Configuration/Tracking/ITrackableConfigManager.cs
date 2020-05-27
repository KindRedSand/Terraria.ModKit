namespace Razorwing.Framework.Configuration.Tracking
{
    /// <summary>
    /// An <see cref="IConfigManager"/> that provides a way to track its config settings.
    /// </summary>
    public interface ITrackableConfigManager : IConfigManager
    {
        /// <summary>
        /// Retrieves all the settings of this <see cref="ConfigManager{T}"/> that are to be tracked for changes.
        /// </summary>
        /// <returns>A list of <see cref="ITrackedSetting"/>.</returns>
        TrackedSettings CreateTrackedSettings();

        /// <summary>
        /// Loads <see cref="Bindable{T}"/>s into <see cref="TrackedSettings"/>.
        /// </summary>
        /// <param name="settings">The settings to load into.</param>
        void LoadInto(TrackedSettings settings);
    }
}
