namespace Dotnet.Script.Server.NuGet
{
    public class PackageQueryResult
    {
        public PackageQueryResult(string id,string description, long? downloadCount, string source, string sourceUrl, string[] versions)
        {
            Id = id;
            Description = description;
            DownloadCount = downloadCount;
            Source = source;
            SourceUrl = sourceUrl;
            Versions = versions;
        }

        public string Id { get; }
        public string Description { get; }
        public long? DownloadCount { get; }
        public string Source { get; }
        public string SourceUrl { get; }
        public string[] Versions { get; }
    }
}