using System.Threading;

namespace Dotnet.Script.Server.CQRS
{
    /// <summary>
    /// Represents a class that implements a 
    /// property containing a <see cref="CancellationToken"/> 
    /// </summary>
    public interface ICancellable
    {
        /// <summary>
        /// Gets or sets the <see cref="CancellationToken"/>.
        /// </summary>
        CancellationToken CancellationToken { get; set; }
    }
}