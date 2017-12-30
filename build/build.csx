#! "netcoreapp2.0"
#r "nuget:Octokit, 0.27.0"
#load "Git.csx"
#load "Changelog.csx"
#load "BuildContext.csx"
#load "DotNet.csx"
#load "FileUtils.csx"
#load "GitHub.csx"
#load "NuGet.csx"
#load "Logger.csx"

DotNet.Build(BuildContext.PathToProjectFolder);
DotNet.Publish(BuildContext.PathToProjectFolder);
DotNet.Pack(BuildContext.PathToProjectFolder, BuildContext.NuGetPackagesFolder);
GitHub.Pack(BuildContext.PathToPublishFolder, BuildContext.GitHubReleaseFolder);

if (Git.IsTagCommit())
{            
    NuGet.Push(BuildContext.NuGetPackagesFolder);    
    GitHub.CreateRelease(BuildContext.GitHubReleaseFolder);       
}