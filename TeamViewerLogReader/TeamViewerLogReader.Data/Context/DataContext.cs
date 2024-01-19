using System;
using System.Data.SqlClient;

namespace TeamViewerLogReader.Data.Context
{
    public class DataContext : IDisposable
    {
        public SqlConnection Connection { get; private set; }

        public DataContext(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            InitializeDatabase();
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        private void InitializeDatabase()
        {
            string script = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserTvLog')
            BEGIN
                CREATE TABLE UserTvLog (
                    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
                    Name NVARCHAR(256) NOT NULL,
                    Surname NVARCHAR(256) NOT NULL,
                    Email NVARCHAR(256) NOT NULL,
                    PasswordHash NVARCHAR(MAX) NOT NULL,
                    PhoneNumber NVARCHAR(50) NULL,
                    Username NVARCHAR(256) NOT NULL,
                    DateCreated DATETIME NOT NULL,
                    IsDeleted BIT NOT NULL,
                    Company NVARCHAR(256) NULL,     
                    Position NVARCHAR(256) NULL 
                );
            END;

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TeamViewerLogEntry]') AND type in (N'U'))
            BEGIN
                CREATE TABLE TeamViewerLogEntry (
                    Id INT PRIMARY KEY IDENTITY,
                    Timestamp DATETIME,
                    ProcessId INT,
                    ThreadId INT,
                    LogLevel NVARCHAR(50),
                    Message NVARCHAR(MAX),
                    UserTvLogId UNIQUEIDENTIFIER,
                    FOREIGN KEY (UserTvLogId) REFERENCES UserTvLog(Id)
                );
            END;

            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLoginHistory]') AND type in (N'U'))
            BEGIN
                CREATE TABLE UserLoginHistory (
                    Id INT PRIMARY KEY IDENTITY,
                    UserTvLogId UNIQUEIDENTIFIER NOT NULL,
                    ComputerName NVARCHAR(255),
                    IpAdress NVARCHAR(255),
                    LoginTimestamp DATETIME NOT NULL,
                    MacAddress NVARCHAR(255),
                    FOREIGN KEY (UserTvLogId) REFERENCES UserTvLog(Id)
                );
            END;
            ";

            using (var command = new SqlCommand(script, Connection))
            {
                command.ExecuteNonQuery();
            }
        }

    }
}
