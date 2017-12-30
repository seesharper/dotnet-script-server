#r "nuget:Octokit, 0.27.0"
#load "Changelog.csx"
#load "FileUtils.csx"
#load "Git.csx"
#load "FileUtils.csx"
#load "Logger.csx"

using Octokit;
public static class GitHub
{
    public static void Pack(string pathToPublishFolder, string githubReleaseFolder)
    {
        ChangeLog.Generate(Path.Combine(githubReleaseFolder, "CHANGELOG.MD"));
        string latestTag = Git.GetLatestTag();
        string projectName = "AutomatedRelease";
        string zipFileName = $"{projectName}.{latestTag}.zip";
        string zipFilePath = Path.Combine(githubReleaseFolder, zipFileName);
        Zip(pathToPublishFolder, zipFilePath);
    }
   
    public static void CreateRelease(string githubReleaseFolder, bool draft = false)
    {
        var repositoryInfo = Git.GetRepositoryInfo();
        var accessToken = System.Environment.GetEnvironmentVariable("GITHUB_REPO_TOKEN");
        var client = new GitHubClient(new ProductHeaderValue(repositoryInfo.ProjectName));
        string latestTag = Git.GetLatestTag();
        var releaseNotes = FileUtils.ReadFile(Path.Combine(githubReleaseFolder,"CHANGELOG.MD"));                        
        var newRelease = new NewRelease(latestTag);    
        newRelease.Name = latestTag;
        newRelease.Body = releaseNotes;
        newRelease.Draft = draft;
        newRelease.Prerelease = latestTag.Contains("-");

        var tokenAuth = new Credentials(accessToken); 
        client.Credentials = tokenAuth;

        var createdRelease = client.Repository.Release.Create(repositoryInfo.Owner, repositoryInfo.ProjectName, newRelease).Result;
        
        var assets = Directory.GetFiles(githubReleaseFolder,"*.zip");
        foreach(var asset in assets)
        {
            var archiveContents = File.OpenRead(asset);
            var assetUpload = new ReleaseAssetUpload() 
            {
                FileName = Path.GetFileName(asset),
                ContentType = "application/zip",
                RawData = archiveContents
            };
            client.Repository.Release.UploadAsset(createdRelease, assetUpload).Wait();
        }        
    }
}