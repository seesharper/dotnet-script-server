using System;
using LightInject;

namespace Dotnet.Script.Server.Stdio.Tests
{
    public class ConfigurableStartup : Startup
    {
        private readonly Action<IServiceContainer> _configureAction;

        public ConfigurableStartup(Action<IServiceContainer> configureAction)
        {
            _configureAction = configureAction;
        }

        public override void ConfigureServices(IServiceContainer container)
        {
            base.ConfigureServices(container);
            _configureAction(container);
        }
    }
}