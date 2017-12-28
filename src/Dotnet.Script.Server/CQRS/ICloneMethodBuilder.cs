using System;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// Represents a class that is capable of dynamically creating 
    /// a method that performs object cloning.    
    /// </summary>
    public interface ICloneMethodBuilder
    {
        /// <summary>
        /// Creates a dynamic method that is used to clone an object of 
        /// type <typeparamref name="T"/> into an instance of a <paramref name="derivedType"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to be cloned.</typeparam>
        /// <param name="derivedType">The type of the derived type to be returned from the clone operation.</param>
        /// <returns>A method capable of cloning an instance of <typeparamref name="T"/> to an instance of the <paramref name="derivedType"/>.</returns>
        Func<T, T> CreateCloneMethod<T>(Type derivedType);
    }
}