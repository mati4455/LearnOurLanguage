using Microsoft.Extensions.Logging;

namespace Model.Core
{
    public class App
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger Logger { get; set; }
        public static ILogger<T> CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
    }
}
