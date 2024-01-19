using System.Text.RegularExpressions;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;
using TeamViewerLogReader.Log.Interfaces;

namespace TeamViewerLogReader.Business
{
    public class LogRederBusiness : ILogRederBusiness
    {
        private readonly ILoggerService _logger;

        const string baseDir = @"C:\Program Files\TeamViewer";
        private readonly ILogEntryRepositoryClickHouse _repository;
        private readonly IUserTvLogBusiness _businessUser;
        public LogRederBusiness(ILogEntryRepositoryClickHouse repository, IUserTvLogBusiness businessUser, ILoggerService logger)
        {
            _repository = repository;
            _businessUser = businessUser;
            _logger = logger;

        }
        public List<TeamViewerLogEntry> ReadLog()
        {
            try
            {
                UserTvLog user = new UserTvLog();
                _logger.LogInformation("Start - FindLogFilePath");
                string path = FindLogFilePath();

                _logger.LogInformation("Start - path name - " + path);
                List<TeamViewerLogEntry> lstLogTV = new List<TeamViewerLogEntry>();

                _logger.LogInformation("Check Last Login");
                user = _businessUser.CheckLastLogin();
                if (user != null)
                {
                    _logger.LogInformation("Found user");
                }
                else
                {
                    user = _businessUser.CreateDefaultUser();
                }
                CheckExistingLogs(path, lstLogTV, user);
                CheckUpdateFileLog(path);

                return lstLogTV;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred in the ReadLog method.", ex);

                throw ex;
            }
            
        }


        private void CheckUpdateFileLog(string path)
        {
            try
            {
                FileSystemWatcher watcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(path),
                    Filter = Path.GetFileName(path),
                    NotifyFilter = NotifyFilters.LastWrite
                };
                watcher.Changed += (source, e) =>
                {
                    var updatedEntries = ReadLogFile(e.FullPath);
                    _repository.AddLogEntries(updatedEntries);
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void CheckExistingLogs(string path, List<TeamViewerLogEntry> lstLogTV, UserTvLog user)
        {
            lstLogTV.AddRange(ReadLogFile(path));
            if (lstLogTV != null && lstLogTV.Count>0)
            {
                lstLogTV.ForEach(log => log.UserTvLogId = user.Id);
                foreach (var item in lstLogTV)
                {
                    if (!_repository.LogEntryExists(item))
                    {
                        _repository.AddLogEntries(lstLogTV);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private static List<TeamViewerLogEntry> ReadLogFile(string filePath)
        {
            List<TeamViewerLogEntry> entries = new List<TeamViewerLogEntry>();

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    List<string> lines = new List<string>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                    entries = ParseLogEntries(lines);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Unable to read the file. It is being used by another process.");
                Console.WriteLine(e.Message);
            }

            return entries;
        }


        private static string FindLogFilePath()
        {
            for (int version = 15; version >= 1; version--)
            {
                string filePath = Path.Combine(baseDir, $"TeamViewer{version}_Logfile.log");
                if (File.Exists(filePath))
                {
                    return filePath;
                }
            }
            string filePath2 = Path.Combine(baseDir, $"TeamViewer_Logfile.log");
            if (File.Exists(filePath2))
            {
                return filePath2;
            }

            throw new FileNotFoundException("No TeamViewer log file found.");
        }

        public static List<TeamViewerLogEntry> ParseLogEntries(List<string> logLines)
        {
            List<TeamViewerLogEntry> logEntries = new List<TeamViewerLogEntry>();
            TeamViewerLogEntry currentEntry = null;

            foreach (var line in logLines)
            {
                // Checks if the line starts with a date ("YYYY/MM/DD" format)
                if (Regex.IsMatch(line, @"^\d{4}/\d{2}/\d{2}"))
                {
                    if (currentEntry != null)
                    {
                        logEntries.Add(currentEntry);
                    }
                    currentEntry = ParseFromLogLine(line);
                }
                else if (currentEntry != null)
                {
                    // Appends the current line to the current logEntry message
                    currentEntry.Message += " " + line.Trim();
                }
            }

            if (currentEntry != null)
            {
                logEntries.Add(currentEntry);
            }

            return logEntries;
        }


        public static TeamViewerLogEntry ParseFromLogLine(string logLine)
        {
            var parts = logLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 5)
            {
                throw new FormatException("Log line is not in the expected format.");
            }

            var timestampString = parts[0] + " " + parts[1];
            var message = string.Join(" ", parts.Skip(5)); //Joins all elements starting from index 5

            var logEntry = new TeamViewerLogEntry
            {
                Timestamp = DateTime.Parse(timestampString),
                ProcessId = int.Parse(parts[2]),
                ThreadId = int.Parse(parts[3]),
                LogLevel = parts[4],
                Message = message
            };

            return logEntry;
        }
    }
}
