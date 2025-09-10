namespace Intuit.Api.Logging
{
    public interface ILogService<T>
    {
        public void LogInfo(string info);
        public void LogWarning(string info);
        public void LogError(string info);
        public void LogCritical(string info);
    }
}
