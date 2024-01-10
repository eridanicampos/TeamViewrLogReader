using NLog;
using TeamViewerLogReader.Log.Interfaces;

namespace TeamViewerLogReader.Log
{

    public class NLogService : ILoggerService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void LogInformation(string message)
        {
            Logger.Info(message);
        }

        public void LogWarning(string message)
        {
            Logger.Warn(message);
        }

        public void LogError(string message, Exception exception)
        {
            Logger.Error(exception, message);
        }
    }
}
