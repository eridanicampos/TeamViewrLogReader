using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Business.Interfaces
{
    public interface ILogRederBusiness
    {
        List<TeamViewerLogEntry> ReadLog();
    }
}
