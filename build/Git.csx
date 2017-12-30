#load "Command.csx"
using System.Text.RegularExpressions;

private static string RemoveNewLine(this string value)
{
    return value.Replace(Environment.NewLine, "");
}

public static class Git
{
    public static RepositoryInfo GetRepositoryInfo()
    {
        var urlToPushOrigin = GetUrlToPushOrigin();
        var match = Regex.Match(urlToPushOrigin, @".*.com\/(.*)\/(.*)\.");
        var owner = match.Groups[1].Value;
        var project = match.Groups[2].Value;
        return new RepositoryInfo(){Owner = owner, ProjectName = project};
    }
    
    
    public static string GetLatestTag()
    {        
        var currentCommitHash = GetCurrentCommitHash();
        var result = Command.Capture("git",$"describe --abbrev=0 --tags {currentCommitHash}");
        return result.RemoveNewLine();;
    }

    public static string GetAccessToken()
    {
        var accessToken = System.Environment.GetEnvironmentVariable("GITHUB_REPO_TOKEN");
        return accessToken;
    }

    public static string GetUrlToPushOrigin()
    {                
        return Command.Capture("git","remote get-url --push origin");
    }

    public static bool IsTagCommit()
    {
        var currentTagHash = GetLatestTagHash();
        var currentCommitHash = GetCurrentCommitHash();
        return currentTagHash == currentCommitHash;
    }

    public static string GetPreviousTag()
    {        
        var previousCommitHash = GetPreviousCommitHash();
        var result = Command.Capture("git",$"describe --abbrev=0 --tags {previousCommitHash}");
        return result.RemoveNewLine();
    }

    public static bool IsOnMaster()
    {
        var currentBranch = GetCurrentBranch();
        return currentBranch == "master";
    }

    private static string GetPreviousCommitHash()
    {
        return Command.Capture("git", "rev-list --tags --skip=1 --max-count=1").RemoveNewLine();        
    }

    private static string GetLatestTagHash()
    {        
        return Command.Capture("git", "rev-list --tags --max-count=1").RemoveNewLine();
    }

    private static string GetCurrentBranch()
    {
        return Command.Capture("git","rev-parse --abbrev-ref HEAD").ToLower().RemoveNewLine();                
    }

    private static string GetCurrentCommitHash()
    {
        return Command.Capture("git", "rev-list --all --max-count=1").RemoveNewLine();                
    }       
}

public class RepositoryInfo 
{
    public string Owner {get;set;}    

    public string ProjectName {get;set;}    
}