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
            logService.LogError($"Application - {message}");
        }

        public AppException(string message, ILogService<LogService> logService, Exception innerException)
            : base(message, innerException)
        {
            logService.LogError($"Application - {message} - [{innerException.Message}] - [{innerException.StackTrace}]");
        }
    }
}
