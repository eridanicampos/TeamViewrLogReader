using ClickHouse.Ado;
using Polly;
using System.Data;
using System.Data.SqlClient;
using TeamViewerLogReader.Data.Context;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Class;
using TeamViewerLogReader.Domain.Repositories;

namespace TeamViewerLogReader.Data.Repositories
{
    public class ClickHouseAsyncRepository : IClickHouseAsyncRepository
    {
        //private readonly IOptions<ConnectionStringClickHouse> _options;
        private readonly ClickHouseConnection _connection;

        //public ClickHouseAsyncRepository(IOptions<ConnectionStringClickHouse> options)
        //{
        //    _options = options;
        //    _connection = new ClickHouseConnection(_options.Value.Connection);
        //    _connection.Open();
        //}

        public ClickHouseAsyncRepository()
        {
            _connection = new ClickHouseConnection("Compress=True;CheckCompressedHash=False;Compressor=lz4;Host=51.140.152.30;Port=9000;Database=cyberenergia_dev;User=analyst;Password=H7VM7knUofbWLGCe5PZd;SocketTimeout=100000;");
            _connection.Open();
        }

        public async Task ExecuteNonQueryWithParametersAsync(string query, IEnumerable<QueryParameters> data)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(() => InternalExecuteNonQueryWithParametersAsync(query, data));
        }


        public async Task InternalExecuteNonQueryWithParametersAsync(string query, IEnumerable<QueryParameters> data)
        {
            var command = _connection.CreateCommand(query);

            List<ClickHouseParameter> parameters = new();
            foreach (var parameter in data)
            {
                command.Parameters.Add(new ClickHouseParameter()
                {
                    ParameterName = parameter.QueryParameter,
                    Value = parameter.Value
                });
            }

            await Task.Run(() => command.ExecuteReader());
        }

        public async Task ExecuteQueryAsync(string query, Action<IDataReader> mappingAction)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(() => InternalExecuteQueryAsync(query, mappingAction));
        }

        public async Task InternalExecuteQueryAsync(string query, Action<IDataReader> mappingAction)
        {
            await Task.Run(() =>
            {
                using var cmd = _connection.CreateCommand(query);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.ReadAll(r =>
                    {
                        mappingAction.Invoke(r);
                    });
                }
            });
        }

        public async Task ExecuteNonQueryAsync(string query)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(() => InternalExecuteNonQueryAsync(query));
        }

        public async Task InternalExecuteNonQueryAsync(string query)
        {
            using var cmd = _connection.CreateCommand(query);
            await Task.Run(() => cmd.ExecuteNonQuery());
        }

        public async Task ExecuteQueryWithParametersAsync(
            string query,
            IEnumerable<QueryParameters> data,
            Action<IDataReader> mappingAction)
        {
            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(() => InternalExecuteQueryWithParametersAsync(query, data, mappingAction));
        }

        public async Task InternalExecuteQueryWithParametersAsync(
            string query,
            IEnumerable<QueryParameters> data,
            Action<IDataReader> mappingAction
            )
        {
            var command = _connection.CreateCommand(query);

            List<ClickHouseParameter> parameters = new();
            foreach (var parameter in data)
            {
                command.Parameters.Add(new ClickHouseParameter()
                {
                    ParameterName = parameter.QueryParameter,
                    Value = parameter.Value
                });
            }

            var reader = await Task.Run(() => command.ExecuteReader());
            while (reader.Read())
            {
                mappingAction.Invoke(reader);
            }

        }
    }
}
