using Serilog;

namespace Intuit.Api.Logging
{
    public class LogService : ILogService<LogService>
    {
        public LogService() { }
        public void LogError(string info)
        {
            Log.Error(info);
        }

        public void LogCritical(string info)
        {
            Log.Fatal(info);
        }

        public void LogInfo(string info)
        {
            Log.Information(info);
        }

        public void LogWarning(string info)
        {
            Log.Warning(info);
        }
    }
}
