using System;
using System.IO;
using Serilog;

namespace QuizGenAI.Services
{
    public static class LoggingService
    {
        public static void Initialize(string logDirectory)
        {
            Directory.CreateDirectory(logDirectory);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.WithProperty("Application", "QuizGenAI")
                .WriteTo.File(
                    Path.Combine(logDirectory, "quizgenai-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14,
                    shared: true)
                .CreateLogger();
        }

        public static void Information(string messageTemplate, params object[] propertyValues)
        {
            Log.Information(messageTemplate, propertyValues);
        }

        public static void Warning(string messageTemplate, params object[] propertyValues)
        {
            Log.Warning(messageTemplate, propertyValues);
        }

        public static void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Error(exception, messageTemplate, propertyValues);
        }

        public static void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            Log.Fatal(exception, messageTemplate, propertyValues);
        }

        public static void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}
