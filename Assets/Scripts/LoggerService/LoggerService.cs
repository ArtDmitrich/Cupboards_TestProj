using System;
using System.IO;
using UnityEngine;

namespace LoggerService
{
    public class LoggerService : ILoggerService
    {
        private readonly string _logFilePath;
        private readonly object _lockObject = new object();

        public LoggerService()
        {
#if !UNITY_EDITOR
        _logFilePath = GetLogFilePath();
        InitializeLogFile();
#endif
        }

        public void Log(string message)
        {
            Debug.Log(message);
            WriteToFile($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
            WriteToFile($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
            WriteToFile($"[ERROR] {DateTime.Now:HH:mm:ss} - {message}");
        }

        public void LogException(Exception exception)
        {
            Debug.LogException(exception);
            WriteToFile($"[EXCEPTION] {DateTime.Now:HH:mm:ss} - {exception}");
        }

        public void ClearOldLogs(int daysToKeep = 7)
        {
            // Удаляем все логи старше 7 дней
#if !UNITY_EDITOR
        try
        {
            var logDirectory = Path.GetDirectoryName(_logFilePath);
            if (Directory.Exists(logDirectory))
            {
                foreach (var file in Directory.GetFiles(logDirectory, "*.log"))
                {
                    if (File.GetLastWriteTime(file) < DateTime.Now.AddDays(-daysToKeep))
                    {
                        File.Delete(file);
                    }
                }
            }
        }
        catch { /* Игнорируем ошибки очистки */ }
#endif
        }

        private string GetLogFilePath()
        {
            string logDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Logs");
            Directory.CreateDirectory(logDir);
            return Path.Combine(logDir, $"game_{DateTime.Now:yyyyMMdd}.log");
        }

        private void InitializeLogFile()
        {
            WriteToFile($"=== Log started at {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
        }

        private void WriteToFile(string message)
        {
#if !UNITY_EDITOR
        lock (_lockObject)
        {
            File.AppendAllText(_logFilePath, message + Environment.NewLine);
        }
#endif
        }
    }
}