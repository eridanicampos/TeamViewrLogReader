using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Class;

namespace TeamViewerLogReader.Domain.Repositories
{
    public interface IClickHouseAsyncRepository
    {
        Task ExecuteNonQueryAsync(string query);
        Task ExecuteNonQueryWithParametersAsync(string query, IEnumerable<QueryParameters> data);
        Task ExecuteQueryAsync(string query, Action<IDataReader> mappingAction); //Non safe please take care using this one
        Task ExecuteQueryWithParametersAsync(string query, IEnumerable<QueryParameters> data, Action<IDataReader> mappingAction);
    }
}
