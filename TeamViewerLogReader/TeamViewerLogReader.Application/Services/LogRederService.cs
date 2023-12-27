using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Service.Interfaces;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Service
{
    public class LogRederService : ILogRederService
    {
        private readonly ILogRederBusiness _business;

        public LogRederService(ILogRederBusiness business)
        {
            _business = business;
        }
        public List<TeamViewerLogEntry> ReadLog()
        {
            return _business.ReadLog();
        }

    }
}
