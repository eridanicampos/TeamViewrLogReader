using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Domain.Repositories
{
    public interface ILogEntryRepositoryClickHouse
    {
        TeamViewerLogEntry AddLogEntry(TeamViewerLogEntry entry);

        bool LogEntryExists(TeamViewerLogEntry entry);
        void AddLogEntries(List<TeamViewerLogEntry> entries);
    }
}
