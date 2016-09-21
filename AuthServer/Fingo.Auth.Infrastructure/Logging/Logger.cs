using System;

namespace Fingo.Auth.Infrastructure.Logging
{
    public class Logger<T> : ILogger<T> where T : class
    {
        private string _callingClassName = string.Empty;

        public Logger()
        {
            SetTypeName();
        }

        public void Log(LogLevel logLevel , string message)
        {
            if (string.IsNullOrEmpty(_callingClassName))
                _callingClassName = "_callingClassName N/A";

            var newMessage = "<" + _callingClassName + "> " + message;

            try
            {
                switch (logLevel)
                {
                    case LogLevel.Information:
                        Serilog.Log.Information(newMessage);
                        break;
                    case LogLevel.Warning:
                        Serilog.Log.Warning(newMessage);
                        break;
                    case LogLevel.Error:
                        Serilog.Log.Error(newMessage);
                        break;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SetTypeName()
        {
            _callingClassName = typeof(T).Name;
        }
    }
}