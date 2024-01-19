using ClickHouse.Ado;
using System;
using System.Data.SqlClient;

namespace TeamViewerLogReader.Data.Context
{
    public class ClickHouseDataContext : IDisposable
    {
        public ClickHouseConnection Connection { get; private set; }

        public ClickHouseDataContext(string connectionString)
        {
            Connection = new ClickHouseConnection(connectionString);
            Connection.Open();
            InitializeDatabase();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        private void InitializeDatabase()
        {
            //string script = @"
            //CREATE TABLE IF NOT EXISTS TeamViewerLogEntry
            //(
            //    Id Int32,
            //    Timestamp DateTime,
            //    ProcessId Int32,
            //    ThreadId Int32,
            //    LogLevel String,
            //    Message String,
            //    UserTvLogId UUID,
            //    INDEX idx_user_tv_log_id (UserTvLogId) TYPE set(0) GRANULARITY 4
            //) ENGINE = MergeTree()
            //ORDER BY (Id);
            //";

            //using (var command = new ClickHouseCommand(Connection))
            //{
            //    command.CommandText = script;
            //    command.ExecuteNonQuery();
            //}
        }
    }
}
