using Microsoft.Extensions.Logging;

namespace FindbookApi
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string path;
        
        public FileLoggerProvider(string path)
        {
            this.path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(path);
        }

        public void Dispose()
        { }
    }
}
