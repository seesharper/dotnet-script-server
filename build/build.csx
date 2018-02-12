#! "netcoreapp2.0"
#load "BuildContext.csx"
#load "nuget:Dotnet.Build, 0.2.1"
#load "nuget:github-changelog, 0.1.2"

using static ChangeLog;
using static ReleaseManagement;


DotNet.Build(BuildContext.PathToProjectFolder);
DotNet.Test(BuildContext.PathToTestProjectFolder);

FileUtils.Zip(BuildContext.PathToPublishFolder,BuildContext.PathToReleaseAsset);
if (BuildEnvironment.IsSecure)
{    
    var generator = ChangeLogFrom("seesharper","dotnet-script-server", BuildEnvironment.GitHubAccessToken).SinceLatestTag();
    if (!Git.Default.IsTagCommit())
    {
        generator = generator.IncludeUnreleased();
    }
    await generator.Generate(BuildContext.PathToReleaseNotes);

    if (Git.Default.IsTagCommit())
    {
        var releaseManager = ReleaseManagerFor("seesharper", "dotnet-script-server", BuildEnvironment.GitHubAccessToken);
        var releaseAssets = new [] { new ZipReleaseAsset(BuildContext.PathToReleaseAsset)};
        await releaseManager.CreateRelease(Git.Default.GetLatestTag(),BuildContext.PathToReleaseNotes, releaseAssets);
    }
}



