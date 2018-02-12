#load "nuget:Dotnet.Build, 0.2.0"

using System.Runtime.CompilerServices;
using static FileUtils;

public static class BuildContext
{
    static BuildContext()
    {
        Root = FileUtils.GetScriptFolder();
                
        var artifactsFolder = CreateDirectory(Root, "Artifacts");                                
        GitHubArtifactsFolder = CreateDirectory(artifactsFolder,"GitHub");        
        PathToReleaseAsset = Path.Combine(GitHubArtifactsFolder,$"Dotnet.Script.Server.{Git.Default.GetLatestTag()}.zip");
        PathToReleaseNotes = Path.Combine(GitHubArtifactsFolder,"ReleaseNotes.md");
        PathToProjectFolder = Path.Combine(Root, "..", "src", "Dotnet.Script.Server.Stdio");
        PathToTestProjectFolder = Path.Combine(Root, "..", "src", "Dotnet.Script.Server.Stdio.Tests");
        
        PathToPublishFolder = Path.Combine(PathToProjectFolder, "bin","Release","netcoreapp2.0","publish");
    }

    public static string Root {get;} 

    public static string GitHubArtifactsFolder {get;} 
        
    public static string PathToProjectFolder {get;}

    public static string PathToTestProjectFolder {get;}

    public static string PathToPublishFolder {get;}
    
    public static string PathToReleaseAsset {get;}
    public static string PathToReleaseNotes {get;}
}