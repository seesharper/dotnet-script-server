using Dotnet.Script.Server.CQRS;
using Dotnet.Script.Server.NuGet;
using LightInject;

namespace Dotnet.Script.Server
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry
                .RegisterQueryHandlers()
                .Register<ISourceRepositoryProviderFactory, SourceRepositoryProviderFactory>(new PerContainerLifetime());
        }
    }
}
