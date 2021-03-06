﻿using System;

namespace Dotnet.Script.Server.Logging
{
    public delegate Logger LogFactory(Type type);

    public delegate void Logger(LogLevel level, string message, Exception exception = null);

    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public static class LogExtensions
    {
        public static Logger CreateLogger<T>(this LogFactory logFactory) => logFactory(typeof(T));

        public static void Debug(this Logger logger, string message) => logger(LogLevel.Debug, message);

        public static void Info(this Logger logger, string message) => logger(LogLevel.Info, message);

        public static void Warning(this Logger logger, string message) => logger(LogLevel.Warning, message);

        public static void Error(this Logger logger, string message, Exception exception = null) => logger(LogLevel.Error, message, exception);

        public static void Fatal(this Logger logger, string message, Exception exception = null) => logger(LogLevel.Fatal, message, exception);
    }
}