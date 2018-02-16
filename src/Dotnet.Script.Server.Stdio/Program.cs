using System.Diagnostics;

namespace Dotnet.Script.Server.Stdio
{
    class Program
    {
        static void Main(string[] args)
        {            
            var application = AppBuilder.Default.Build();            
            application.Run();            
        }
    }
}
