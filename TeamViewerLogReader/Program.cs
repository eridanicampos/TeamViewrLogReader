using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace TeamViewerLogReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            //var logMonitor = serviceProvider.GetService<LogMonitor>();


            MonitorFileTeamViewer();
        }


        private static void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<ILogReader, LogReader>();
            //services.AddTransient<LogMonitor>();
        }   

        private static void MonitorFileTeamViewer()
        {
            string path = FindLogFilePath();
            List<TeamViewerLogEntry> lstLogTV = new List<TeamViewerLogEntry>();
            try
            {
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = Path.GetDirectoryName(path);
                    watcher.Filter = Path.GetFileName(path);

                    watcher.NotifyFilter = NotifyFilters.LastWrite;

                    watcher.Changed += (source, e) => ReadLogFile(e.FullPath, lstLogTV);

                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine("Press 'e' to exit.");
                    while (Console.Read() != 'e') ;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private static void ReadLogFile(string filePath, List<TeamViewerLogEntry> lstLogTV)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var logEntry = TeamViewerLogEntry.ParseFromLogLine(line);
                        lstLogTV.Add(logEntry);

                        //delete this if it is necessary
                        Console.WriteLine(line);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("Unable to read the file. It is being used by another process.");
                Console.WriteLine(e.Message);
            }
        }


        private static string FindLogFilePath()
        {
            string baseDir = @"C:\Program Files\TeamViewer";
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


    }

    internal class TeamViewerLogEntry
    {
        public DateTime Timestamp { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }

        public static TeamViewerLogEntry ParseFromLogLine(string logLine)
        {
            var parts = logLine.Split(new[] { ' ' }, 5);

            var logEntry = new TeamViewerLogEntry
            {
                Timestamp = DateTime.Parse(parts[0] + " " + parts[1]),
                ProcessId = int.Parse(parts[2]),
                ThreadId = int.Parse(parts[3]),
                LogLevel = parts[4],
                Message = parts[5]
            };

            return logEntry;
        }
    }
}
