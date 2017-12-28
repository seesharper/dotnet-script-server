using Dotnet.Script.Server.CQRS;

namespace Dotnet.Script.Server.NuGet
{
    public class PackageQuery : IQuery<PackageQueryResult[]>
    {
        public string PackageId { get; set; }
    }
}