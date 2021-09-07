using Microsoft.Extensions.Logging;

namespace CardFile.WebAPI.Logger
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));

            return factory;
        }
    }
}