using System;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// Represents a class that is capable of building a <see cref="Type"/> that implements <see cref="ICancellable"/>
    /// </summary>
    public interface ICancellableTypeBuilder
    {
        /// <summary>
        /// Creates a new <see cref="Type"/> that inherits from 
        /// </summary>
        /// <param name="parentType">The <see cref="Type"/> for which the new parentType inherits from.</param>
        /// <returns>A <see cref="Type"/> that inherits from <paramref name="parentType"/> and implements <see cref="ICancellable"/>.</returns>
        Type CreateCancellableType(Type parentType);
    }
}