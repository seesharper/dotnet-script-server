using System;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;
using Dotnet.Script.Server.CQRS;
using LightInject;

namespace Dotnet.Script.Server
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterQueryHandlers();
        }
    }
}
