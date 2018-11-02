using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchemaValidator
{
    public class Logger
    {
        // https://stackoverflow.com/a/19538654/6345585
        public static ErrorAppender Setup(string file = null)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            // clear in case this is not the first run
            hierarchy.ResetConfiguration();

            PatternLayout patternLayout = new PatternLayout
            {
                // ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
                ConversionPattern = "%date %-5level - %message%newline"
            };
            patternLayout.ActivateOptions();

            if (file != null)
            {
                RollingFileAppender roller = new RollingFileAppender
                {
                    AppendToFile = false,
                    File = file,
                    Layout = patternLayout,
                    MaxSizeRollBackups = 5,
                    MaximumFileSize = "1GB",
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    StaticLogFileName = true,
                    Threshold = Level.Warn
                };
                roller.ActivateOptions();
                hierarchy.Root.AddAppender(roller);
            }

            var appender = new ErrorAppender();
            appender.ActivateOptions();
            hierarchy.Root.AddAppender(appender);

            // add console appender to see what is happenning
            var csl = new ColoredConsoleAppender
            {
                Layout = patternLayout,
                Name = "ConsoleAppender",
                Threshold = Level.Info
            };
            csl.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Error,
                ForeColor = ColoredConsoleAppender.Colors.Red
            });
            csl.AddMapping(new ColoredConsoleAppender.LevelColors
            {
                Level = Level.Warn,
                ForeColor = ColoredConsoleAppender.Colors.Yellow
            });
            csl.ActivateOptions();
            hierarchy.Root.AddAppender(csl);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            return appender;
        }
    }

    public class ErrorAppender: MemoryAppender
    {
        public ErrorAppender()
        {
            Threshold = Level.Warn;
        }
        public IEnumerable<LogMessage> Errors => GetEvents()
            .Where(e => e.Level == Level.Error)
            .Select(e => new LogMessage { Message = e.RenderedMessage, Exception = e.ExceptionObject });

        public IEnumerable<LogMessage> Warnings => GetEvents()
            .Where(e => e.Level == Level.Warn)
            .Select(e => new LogMessage { Message = e.RenderedMessage, Exception = e.ExceptionObject });

    }

    public struct LogMessage
    {
        public string Message;
        public Exception Exception;
    }
}
