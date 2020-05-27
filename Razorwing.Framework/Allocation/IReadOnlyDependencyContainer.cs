using System;

namespace Razorwing.Framework.Allocation
{
    /// <summary>
    /// Read-only interface into a dependency container capable of injective and retrieving dependencies based
    /// on types.
    /// </summary>
    public interface IReadOnlyDependencyContainer
    {
        /// <summary>
        /// Retrieves a cached dependency of <paramref name="type"/> if it exists and null otherwise.
        /// </summary>
        /// <param name="type">The dependency type to query for.</param>
        /// <returns>The requested dependency, or null if not found.</returns>
        object Get(Type type);

        /// <summary>
        /// Injects dependencies into the given instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance to inject dependencies into.</typeparam>
        /// <param name="instance">The instance to inject dependencies into.</param>
        void Inject<T>(T instance) where T : class;
    }

    public static class ReadOnlyDependencyContainerExtensions
    {
        /// <summary>
        /// Retrieves a cached dependency of type <typeparamref name="T"/> if it exists, and null otherwise.
        /// </summary>
        /// <typeparam name="T">The dependency type to query for.</typeparam>
        /// <param name="container">The <see cref="IReadOnlyDependencyContainer"/> to query.</param>
        /// <returns>The requested dependency, or null if not found.</returns>
        public static T Get<T>(this IReadOnlyDependencyContainer container)
            where T : class
            => (T)container.Get(typeof(T));

        /// <summary>
        /// Retrieves a cached dependency of type <typeparamref name="T"/> if it exists, and default(<typeparamref name="T"/>) otherwise.
        /// </summary>
        /// <typeparam name="T">The dependency type to query for.</typeparam>
        /// <param name="container">The <see cref="IReadOnlyDependencyContainer"/> to query.</param>
        /// <returns>The requested dependency, or default(<typeparamref name="T"/>) if not found.</returns>
        internal static T GetValue<T>(this IReadOnlyDependencyContainer container)
        {
            var result = container.Get(typeof(T));
            if (result == null)
                return default;
            return (T)container.Get(typeof(T));
        }

        /// <summary>
        /// Tries to retrieve a cached dependency of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="container">The <see cref="IReadOnlyDependencyContainer"/> to query.</param>
        /// <param name="value">The requested dependency, or null if not found.</param>
        /// <typeparam name="T">The dependency type to query for.</typeparam>
        /// <returns>Whether the requested dependency existed.</returns>
        public static bool TryGet<T>(this IReadOnlyDependencyContainer container, out T value)
            where T : class
        {
            value = container.Get<T>();
            return value != null;
        }
    }
}
