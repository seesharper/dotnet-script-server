using System;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using Dotnet.Script.Server.Logging;
using LightInject;

namespace Dotnet.Script.Server.Stdio
{
    public class Startup
    {        
        public virtual void ConfigureServices(IServiceContainer container)
        {            
            container.RegisterFrom<CompositionRoot>();
            container.Register<StdioApplication>();
            container.RegisterInstance(Console.In, "input");
            container.RegisterInstance(Console.Out, "output");
            AddStdErrorLogging(container);            
        }

        private static void AddStdErrorLogging(IServiceContainer container)
        {
            Logger LogFactory(Type type) => (level, message, exception) => { Console.Error.WriteLine(message); };
            container.RegisterConstructorDependency((factory, parameterInfo) => LogFactory(parameterInfo.Member.DeclaringType));
        }
    }

    public class AppBuilder
    {
        public static AppBuilder Default = new AppBuilder(new Startup());

        private readonly Startup _startup;
          
        private AppBuilder(Startup startup)
        {
            _startup = startup;
        }

        public AppBuilder UseStartup<TStartup>(TStartup startup) where TStartup : Startup
        {
            return new AppBuilder(startup);
        }

        public StdioApplication Build()
        {
            var container = new ServiceContainer();
            _startup.ConfigureServices(container);
            return container.GetInstance<StdioApplication>();
        }
    }
}