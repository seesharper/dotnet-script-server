using System;
using System.Collections.Concurrent;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// An <see cref="ICancellableTypeBuilder"/> decorator 
    /// that caches the dynamically created cancellable type.
    /// </summary>
    public class CachedCancellableTypeBuilder : ICancellableTypeBuilder
    {
        private readonly ICancellableTypeBuilder _cancellableTypeBuilder;
        private readonly ConcurrentDictionary<Type, Type> _cache = new ConcurrentDictionary<Type, Type>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCancellableTypeBuilder"/> class.
        /// </summary>
        /// <param name="cancellableTypeBuilder">The <see cref="ICancellableTypeBuilder"/> used to dynamically 
        /// create a type implementing <see cref="ICancellable"/>.</param>
        public CachedCancellableTypeBuilder(ICancellableTypeBuilder cancellableTypeBuilder)
        {
            _cancellableTypeBuilder = cancellableTypeBuilder;
        }

        /// <summary>
        /// Creates a new <see cref="Type"/> that inherits from 
        /// </summary>
        /// <param name="parentType">The <see cref="Type"/> for which the new parentType inherits from.</param>
        /// <returns>A <see cref="Type"/> that inherits from <paramref name="parentType"/> and implements <see cref="ICancellable"/>.</returns>
        public Type CreateCancellableType(Type parentType)
        {
            return _cache.GetOrAdd(parentType, _cancellableTypeBuilder.CreateCancellableType);
        }
    }
}