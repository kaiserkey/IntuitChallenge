using Intuit.Api.Logging;

namespace Intuit.Api.Exceptions
{
    /// <summary>
    /// Custom exception for application-level errors.
    /// </summary>
    internal class AppException : Exception
    {
        public AppException() : base() { }

        public AppException(string message, ILogService<LogService> logService) : base(message)
        {
            logService.LogError($"\nApplication - {message}");
        }

        public AppException(string message, ILogService<LogService> logService, Exception innerException)
            : base(message, innerException)
        {
            logService.LogError($"\nApplication - {message} - [{innerException.Message}] - [{innerException.StackTrace}]");
        }
    }
}
