using System.Data.SqlClient;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;

namespace TeamViewerLogReader.Data.Repositories
{
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly DataContext _context;

        public LogEntryRepository(DataContext context)
        {
            _context = context;
        }

        public TeamViewerLogEntry AddLogEntry(TeamViewerLogEntry entry)
        {
            string query = "INSERT INTO TeamViewerLogEntry (Timestamp, ProcessId, ThreadId, LogLevel, Message, UserTvLogId) " +
                           "OUTPUT INSERTED.ID " +
                           "VALUES (@Timestamp, @ProcessId, @ThreadId, @LogLevel, @Message, @UserTvLogId);";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Timestamp", entry.Timestamp);
                command.Parameters.AddWithValue("@ProcessId", entry.ProcessId);
                command.Parameters.AddWithValue("@ThreadId", entry.ThreadId);
                command.Parameters.AddWithValue("@LogLevel", entry.LogLevel);
                command.Parameters.AddWithValue("@Message", entry.Message);
                command.Parameters.AddWithValue("@UserTvLogId", entry.UserTvLogId);

                entry.Id = (int)command.ExecuteScalar();
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

            string query = "SELECT COUNT(1) FROM TeamViewerLogEntry WHERE Timestamp = @Timestamp AND Message = @Message;";

            using (var command = new SqlCommand(query, _context.Connection))
            {
                command.Parameters.AddWithValue("@Timestamp", entry.Timestamp);
                command.Parameters.AddWithValue("@Message", entry.Message);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }



    }
}
