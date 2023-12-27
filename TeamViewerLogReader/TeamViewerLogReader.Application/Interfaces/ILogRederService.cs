using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Service.Interfaces
{
    public interface ILogRederService
    {
        public List<TeamViewerLogEntry> ReadLog();
    }
}
