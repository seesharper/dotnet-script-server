namespace Dotnet.Script.Server.NuGet
{
    public class PackageQueryResult
    {
        public PackageQueryResult(string id)
        {
            Id = id;            
        }

        public string Id { get; }
    }
}