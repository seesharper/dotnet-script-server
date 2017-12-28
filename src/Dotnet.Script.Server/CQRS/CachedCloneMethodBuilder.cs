using System;
using System.Collections.Concurrent;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// An <see cref="ICloneMethodBuilder"/> decorator that caches 
    /// the dynamically created method used for object cloning.
    /// </summary>
    public class CachedCloneMethodBuilder : ICloneMethodBuilder
    {
        private readonly ICloneMethodBuilder _cloneMethodBuilder;

        private readonly ConcurrentDictionary<Type, Delegate> _cache = new ConcurrentDictionary<Type, Delegate>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedCloneMethodBuilder"/> class.
        /// </summary>
        /// <param name="cloneMethodBuilder">The target <see cref="ICloneMethodBuilder"/>.</param>
        public CachedCloneMethodBuilder(ICloneMethodBuilder cloneMethodBuilder)
        {
            _cloneMethodBuilder = cloneMethodBuilder;
        }

        /// <summary>
        /// Creates a dynamic method that is used to clone an object of 
        /// type <typeparamref name="T"/> into an instance of a <paramref name="derivedType"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be cloned.</typeparam>
        /// <param name="derivedType">The type of the derived type to be returned from the clone operation.</param>
        /// <returns>A method capable of cloning an instance of <typeparamref name="T"/> to an instance of the <paramref name="derivedType"/>.</returns>
        public Func<T, T> CreateCloneMethod<T>(Type derivedType)
        {
            return (Func<T, T>)_cache.GetOrAdd(derivedType, type => _cloneMethodBuilder.CreateCloneMethod<T>(type));            
        }
    }
}