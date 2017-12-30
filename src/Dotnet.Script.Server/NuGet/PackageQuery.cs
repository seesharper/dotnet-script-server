using Dotnet.Script.Server.CQRS;

namespace Dotnet.Script.Server.NuGet
{
    public class PackageQuery : IQuery<PackageQueryResult[]>
    {
        public PackageQuery(string searchTerm, string rootFolder, bool includePreRelease)
        {
            SearchTerm = searchTerm;
            RootFolder = rootFolder;
            IncludePreRelease = includePreRelease;
        }

        public string SearchTerm { get; }

        public string RootFolder { get; }

        public bool IncludePreRelease { get; }
    }
}