using Microsoft.Extensions.Logging;

namespace CardFile.WebAPI.Logger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _path;

        public FileLoggerProvider(string _path)
        {
            this._path = _path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }

        public void Dispose() { }
    }
}