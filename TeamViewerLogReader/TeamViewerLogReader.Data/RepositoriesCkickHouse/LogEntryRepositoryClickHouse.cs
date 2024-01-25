using ClickHouse.Ado;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;

namespace TeamViewerLogReader.Data.Repositories
{
    public class LogEntryRepositoryClickHouse : ILogEntryRepositoryClickHouse
    {
        private readonly IClickHouseAsyncRepository _clickHouseAsyncRepository;
        private readonly ClickHouseDataContext _context;
        private int count = 0;
        public LogEntryRepositoryClickHouse(ClickHouseDataContext context, IClickHouseAsyncRepository clickHouseAsyncRepository)
        {
            _context = context;
            _clickHouseAsyncRepository = clickHouseAsyncRepository;
        }

        public TeamViewerLogEntry AddLogEntry(TeamViewerLogEntry entry)
        {
            try
            {
                var message = entry.Message.Replace("'", "''");

                string query = @$"INSERT INTO cyberenergia_dev.teamViewerLogEntry (timestamp, processId, threadId, logLevel, message, userTvLogId) 
                           VALUES (toDate('{entry.Timestamp}'), {entry.ProcessId}, {entry.ThreadId}, '{entry.LogLevel}', '{message}', '{entry.UserTvLogId}');";

                _clickHouseAsyncRepository.ExecuteQueryAsync(query, reader => {});
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
            try
            {
                int existCount = 0;

                var message = entry.Message.Replace("'", "''");
                string query = @$"SELECT COUNT(1) FROM cyberenergia_dev.teamViewerLogEntry
                 WHERE timestamp = toDate('{entry.Timestamp}') AND message = '{message}';";

                _clickHouseAsyncRepository.ExecuteQueryAsync(query, reader =>
                {
                    existCount = int.Parse(reader.GetString(0));                       
                });
                return existCount == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
