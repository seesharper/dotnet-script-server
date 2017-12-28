using System.Threading.Tasks;
using Dotnet.Script.Server.CQRS;

namespace Dotnet.Script.Server.NuGet
{
    public class PackageSearchQueryHandler : IQueryHandler<PackageQuery, PackageQueryResult[]>
    {
        public async Task<PackageQueryResult[]> HandleAsync(PackageQuery query)
        {
            return new[]{new PackageQueryResult("LightInject","5.1.1")};
        }
    }
}