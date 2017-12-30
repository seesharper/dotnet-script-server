using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dotnet.Script.Server.Logging;
using NuGet.Common;
using LogLevel = NuGet.Common.LogLevel;

namespace Dotnet.Script.Server.NuGet
{
    public class NuGetLogger : LoggerBase
    {
        private readonly Logger _logger;

        private static Dictionary<LogLevel, Action<Logger, string>> _logActions = new Dictionary<LogLevel, Action<Logger, string>>();

        static NuGetLogger()
        {
            _logActions.Add(LogLevel.Debug, (logger, message) => logger.Debug(message));
            _logActions.Add(LogLevel.Verbose, (logger, message) => logger.Debug(message));
            _logActions.Add(LogLevel.Information, (logger, message) => logger.Info(message));
            _logActions.Add(LogLevel.Minimal, (logger, message) => logger.Info(message));
            _logActions.Add(LogLevel.Warning, (logger, message) => logger.Warning(message));
            _logActions.Add(LogLevel.Error, (logger, message) => logger.Error(message));
        }

        public NuGetLogger(Logger logger)
        {
            _logger = logger;
        }

        public override void Log(ILogMessage message)
        {
            _logActions[message.Level](_logger, message.Message);
        }

        public override Task LogAsync(ILogMessage message)
        {
            Log(message);
            return Task.CompletedTask;
        }
    }
}