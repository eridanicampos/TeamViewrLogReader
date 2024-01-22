using ClickHouse.Ado;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;

namespace TeamViewerLogReader.Data.Repositories
{
    public class LogEntryRepositoryClickHouse : ILogEntryRepositoryClickHouse
    {
        private readonly ClickHouseDataContext _context;

        public LogEntryRepositoryClickHouse(ClickHouseDataContext context)
        {
            _context = context;
        }

        public TeamViewerLogEntry AddLogEntry(TeamViewerLogEntry entry)
        {
            try
            {
                var message = entry.Message.Replace("'", "\'");

                string query = @$"INSERT INTO cyberenergia_dev.teamViewerLogEntry (timestamp, processId, threadId, logLevel, message, userTvLogId) 
                           VALUES (toDate('{entry.Timestamp}'), {entry.ProcessId}, {entry.ThreadId}, '{entry.LogLevel}', '{message}', '{entry.UserTvLogId}');";

                //string query = @"INSERT INTO cyberenergia_dev.teamViewerLogEntry 
                //     (timestamp, processId, threadId, logLevel, message, userTvLogId) 
                //     VALUES (toDate('@Timestamp'), @ProcessId, @ThreadId, @LogLevel, @Message, @UserTvLogId);";



                using (var command = new ClickHouseCommand(_context.Connection, query))
                {
                    //command.Parameters.Add("@Timestamp", entry.Timestamp);
                    //command.Parameters.Add("@ProcessId", entry.ProcessId);
                    //command.Parameters.Add("@ThreadId", entry.ThreadId);
                    //command.Parameters.Add("@LogLevel", entry.LogLevel);
                    //command.Parameters.Add("@Message", entry.Message);
                    //command.Parameters.Add("@UserTvLogId", entry.UserTvLogId);

                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return entry;
        }

        public void AddLogEntries(List<TeamViewerLogEntry> entries)
        {
            foreach (var entry in entries)
            {
                AddLogEntry(entry);
            }
        }

        public bool LogEntryExists(TeamViewerLogEntry entry)
        {
            string query = @$"SELECT COUNT(1) FROM cyberenergia_dev.teamViewerLogEntry
                 WHERE timestamp = toDate('{entry.Timestamp}') AND message = '{entry.Message}';";

            using (var command = new ClickHouseCommand(_context.Connection, query))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return (reader.IsDBNull(0)) ? false : reader.GetInt32(0) > 0;
                    }
                }
            }

            return false;
        }
    }
}
