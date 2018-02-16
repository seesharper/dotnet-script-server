using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dotnet.Script.Server.CQRS;
using Dotnet.Script.Server.Logging;
using NuGet.Protocol.Core.Types;

namespace Dotnet.Script.Server.NuGet
{
    public class PackageSearchQueryHandler : IQueryHandler<PackageQuery, PackageQueryResult[]>
    {
        private readonly ISourceRepositoryProviderFactory _sourceRepositoryProviderFactory;
        private readonly Logger _logger;

        public PackageSearchQueryHandler(ISourceRepositoryProviderFactory sourceRepositoryProviderFactory, Logger logger)
        {
            _sourceRepositoryProviderFactory = sourceRepositoryProviderFactory;
            _logger = logger;
        }

        public async Task<PackageQueryResult[]> HandleAsync(PackageQuery query, CancellationToken cancellationToken)
        {
            var sourceRepositoryProvider = _sourceRepositoryProviderFactory.CreateProvider(query.RootFolder);
            var packages = new HashSet<PackageQueryResult>();
            try
            {
                foreach (var sourceRepository in sourceRepositoryProvider.GetRepositories())
                {
                    var packageSearchResource = await sourceRepository.GetResourceAsync<PackageSearchResource>(cancellationToken);
                    var results = await packageSearchResource.SearchAsync(query.SearchTerm, new SearchFilter(query.IncludePreRelease), 0,
                        int.MaxValue, new NuGetLogger(_logger), cancellationToken);
                    foreach (var result in results.ToArray())
                    {
                        try
                        {                            
                            var versionInfos = await result.GetVersionsAsync();
                            var versions = versionInfos.Select(vi => vi.Version).OrderByDescending(v => v).Select(v => v.ToString()).ToArray();                            
                            packages.Add(new PackageQueryResult(result.Identity.Id,result.Description, result.DownloadCount,sourceRepository.PackageSource.Name, sourceRepository.PackageSource.Source, versions));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return packages.ToArray();
        }
    }
}