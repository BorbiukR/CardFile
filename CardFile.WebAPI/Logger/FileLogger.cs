using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CardFile.WebAPI.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private static readonly object _lock = new object();

        public FileLogger(string path)
        {
            _filePath = path;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(_filePath, logLevel + formatter(state, exception) + Environment.NewLine + DateTime.Now);
                }
            }
        }

        public IDisposable BeginScope<TState>(TState state) => null;
 
        public bool IsEnabled(LogLevel logLevel) => true;       
    }
}