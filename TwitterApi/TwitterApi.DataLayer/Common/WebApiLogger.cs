using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace TwitterApi.DataLayer.Common
{
    public class WebApiLogger
    {
        public static bool Configured { get; protected set; }
        public static ILog Instance;
        public static bool EnableDebugLevel;
        public static bool EnableLogFieldsValidationException;

        static WebApiLogger()
        {
            Configured = false;
            EnableDebugLevel = false;
            EnableLogFieldsValidationException = false;
        }

        public static void Configure(string loggerName, string logFileName, bool isRolling)
        {
            if (Configured) return;
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(Assembly.GetAssembly(typeof(WebApiLogger)));
            ILog log = LogManager.GetLogger(hierarchy.Name, loggerName);
            PatternLayout patternLayout = new PatternLayout { ConversionPattern = "%date %-5level - %message%newline" };
            patternLayout.ActivateOptions();
            Logger l = (Logger)log.Logger;
            if (isRolling)
            {
                RollingFileAppender roller = new RollingFileAppender
                {
                    LockingModel = new FileAppender.MinimalLock(),
                    Layout = patternLayout,
                    AppendToFile = true,
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    MaxSizeRollBackups = 4,
                    MaximumFileSize = "6MB",
                    StaticLogFileName = true,
                    File = logFileName
                };
                roller.ActivateOptions();
                l.AddAppender(roller);
            }
            else
            {
                FileAppender appender = new FileAppender
                {
                    LockingModel = new FileAppender.MinimalLock(),
                    Layout = patternLayout,
                    AppendToFile = true,
                    File = logFileName
                };
                appender.ActivateOptions();
                l.AddAppender(appender);
            }
            l.Level = Level.All;
            hierarchy.Configured = true;
            Instance = log;
            Configured = true;
        }

        public static void Info(string message)
        {
            if (message != null) Instance.Info(message);
        }

        public static void Debug(string message)
        {
            if (EnableDebugLevel) Instance.Debug(message);
        }

        public static void DebugFormat(string message, params object[] args)
        {
            if (EnableDebugLevel) Instance.DebugFormat(message, args);
        }

        public static void LogException(Exception ex)
        {
            Instance.Error(ex.ToString());
        }

        public static void DebugLogException(Exception ex)
        {
            if (EnableDebugLevel)
            {
                LogException(ex);
            }
        }
    }
}
