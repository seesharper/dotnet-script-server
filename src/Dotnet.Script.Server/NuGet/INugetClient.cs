using System.Collections.Generic;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace Dotnet.Script.Server.NuGet
{
    public interface ISourceRepositoryProviderFactory
    {
        ISourceRepositoryProvider CreateProvider(string rootFolder);
    }

    public class SourceRepositoryProviderFactory : ISourceRepositoryProviderFactory
    {
        public ISourceRepositoryProvider CreateProvider(string rootFolder)
        {
            var settings = global::NuGet.Configuration.Settings.LoadDefaultSettings(rootFolder);
            return new SourceRepositoryProvider(settings, Repository.Provider.GetCoreV3());
        }
    }    
}