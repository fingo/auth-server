namespace Fingo.Auth.Infrastructure.Logging
{
    public interface ILogger<T> where T : class
    {
        void Log(LogLevel logLevel , string message);
    }
}