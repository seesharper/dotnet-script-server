using System;
using System.Threading;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// Provides extension methods that allows setting and retrieving an <see cref="CancellationToken"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        private static readonly ICancellableObjectFactory CancellableObjectFactory = new CancellableObjectFactory();


        /// <summary>
        /// Returns an instance of <typeparamref name="T"/> that exposed a <see cref="CancellationToken"/> by implementing <see cref="ICancellable"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to become cancellable.</typeparam>
        /// <param name="target">The target instance.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to be associated with the cancellable instance.</param>
        /// <returns>An instance of <typeparamref name="T"/> that implements <see cref="ICancellable"/>.</returns>
        public static T WithCancellationToken<T>(this T target, CancellationToken cancellationToken) where T: new()
        {
            if (target is ICancellable)
            {
                throw new ArgumentOutOfRangeException(nameof(target),"Already implements ICancellable");
            }
            return CancellableObjectFactory.GetCancellableObject(target, cancellationToken);
        }

        /// <summary>
        /// Gets the <see cref="CancellationToken"/> associated with the <paramref name="command"/>.
        /// </summary>
        /// <typeparam name="T">The type of object for which to return a <see cref="CancellationToken"/>.</typeparam>
        /// <param name="target">The target instance.</param>
        /// <returns></returns>
        public static CancellationToken GetCancellationToken<T>(this T target)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            // Interface is implemented at runtime
            if (target is ICancellable cancellable)
            {
                return cancellable.CancellationToken;
            }

            return default(CancellationToken);
        }
    }


    public interface ICancellableObjectFactory
    {
        T GetCancellableObject<T>(T targetObject, CancellationToken cancellationToken);
    }

    public class CancellableObjectFactory : ICancellableObjectFactory
    {
        
        private readonly ICancellableTypeBuilder _cancellableTypeBuilder;
        private readonly ICloneMethodBuilder _cloneMethodBuilder;

        public CancellableObjectFactory() : this(new CachedCancellableTypeBuilder(new CancellableTypeBuilder()),
            new CachedCloneMethodBuilder(new CloneMethodBuilder()))
        {            
        }

        public CancellableObjectFactory(ICancellableTypeBuilder cancellableTypeBuilder, ICloneMethodBuilder cloneMethodBuilder)
        {
            _cancellableTypeBuilder = cancellableTypeBuilder;
            _cloneMethodBuilder = cloneMethodBuilder;
        }
        


        public T GetCancellableObject<T>(T targetObject, CancellationToken cancellationToken)
        {
            var cancellableType = _cancellableTypeBuilder.CreateCancellableType(typeof(T));
            var cancellableCommand = _cloneMethodBuilder.CreateCloneMethod<T>(cancellableType)(targetObject);
            ((ICancellable)cancellableCommand).CancellationToken = cancellationToken;
            return cancellableCommand;
        }
    }
}