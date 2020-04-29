using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace FindbookApi
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory, string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            factory.AddProvider(new FileLoggerProvider(path));
            return factory;
        }
    }
}
