namespace Dotnet.Script.Server.NuGet
{
    public class PackageQueryResult
    {
        public PackageQueryResult(string id, string version)
        {
            Id = id;
            Version = version;
        }

        public string Id { get; }

        public string Version { get; }
    }
}