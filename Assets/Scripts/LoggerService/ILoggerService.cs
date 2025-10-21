using System;

namespace LoggerService
{
    public interface ILoggerService
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
        void ClearOldLogs(int daysToKeep = 7);
    }
}