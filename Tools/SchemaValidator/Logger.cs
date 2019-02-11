using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Xbim.Common;

namespace SchemaValidator
{
    public class Logger
    {
        // https://stackoverflow.com/a/19538654/6345585
        public static MemoryLog Setup(string file = null)
        {
            var config = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.ColoredConsole();


            if (file != null)
            {
                config.WriteTo.File(file);
            }

            var memLog = new MemoryLog();
            var memLogProvider = new MemoryLogProvider(memLog);
            
            Log.Logger = config.CreateLogger();

            var lf = new LoggerFactory().AddSerilog();
            lf.AddProvider(memLogProvider);
            var log = lf.CreateLogger("The Log");

            // set up xBIM logging. It will use your providers.
            XbimLogging.LoggerFactory = lf;

            return memLog;
        }
    }

    public class MemoryLogProvider : ILoggerProvider
    {
        private readonly MemoryLog log;

        public MemoryLogProvider(MemoryLog log)
        {
            this.log = log;
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return log;
        }

        public void Dispose()
        {
            
        }
    }

    public class MemoryLog: Microsoft.Extensions.Logging.ILogger
    {
        public IEnumerable<LogMessage> Errors => _events
            .Where(e => e.Level == LogLevel.Error)
            .Select(e => new LogMessage { Message = e.Message, Exception = e.Exception });

        public IEnumerable<LogMessage> Warnings => _events
            .Where(e => e.Level == LogLevel.Warning)
            .Select(e => new LogMessage { Message = e.Message, Exception = e.Exception });

        public IDisposable BeginScope<TState>(TState state)
        {
            return new Scope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Warning;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _events.Add(new LogEvent { Level = logLevel, Message = formatter(state, exception), Exception = exception });
        }

        private List<LogEvent> _events = new List<LogEvent>();

        private class LogEvent
        {
            public LogLevel Level;
            public string Message;
            public Exception Exception;
        }

        private class Scope : IDisposable
        {
            public void Dispose()
            {
            }
        }
    }

    public struct LogMessage
    {
        public string Message;
        public Exception Exception;
    }
}
