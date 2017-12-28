using System;
using System.Diagnostics;
using System.Threading;
using LightInject;

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
