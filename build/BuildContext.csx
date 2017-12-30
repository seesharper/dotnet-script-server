#load "FileUtils.csx"
using System.Runtime.CompilerServices;

public static class BuildContext
{
    static BuildContext()
    {
        Root = Path.GetDirectoryName(GetScriptPath());
        var artifactsFolder = Path.Combine(Root,"Artifacts");
        FileUtils.CreateDirectory(Path.Combine(Root,"Artifacts"));
        GitHubReleaseFolder = Path.Combine(artifactsFolder,"GitHubRelease");
        FileUtils.CreateDirectory(GitHubReleaseFolder);
        NuGetPackagesFolder = Path.Combine(artifactsFolder,"NuGet");
        FileUtils.CreateDirectory(NuGetPackagesFolder);
        ChocolateyPackagesFolder = Path.Combine(artifactsFolder,"Chocolatey");
        FileUtils.CreateDirectory(ChocolateyPackagesFolder);        
        PathToProjectFolder = Path.Combine(Root, "../");
        PathToPublishFolder = Path.Combine(PathToProjectFolder, "bin/Release/netstandard2.0/publish");
    }

    public static string Root {get;} 

    public static string GitHubReleaseFolder {get;set;} 

    public static string NuGetPackagesFolder {get;set;}

    public static string ChocolateyPackagesFolder {get;set;} 

    public static string PathToProjectFolder {get;set;}

    public static string PathToPublishFolder {get;set;}
    static string GetScriptPath([CallerFilePath] string path = null) => path;    
}