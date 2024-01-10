using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Log.Interfaces;
using TeamViewerLogReader.Service.Interfaces;

namespace TeamViewerLogReader.ConsoleApp
{

    public class LogMonitor
    {
        private readonly ILoggerService _logger;
        private readonly ILogRederService _service;

        public LogMonitor(ILogRederService service, ILoggerService logger)
        {
            _service = service;
            _logger = logger;
        }

        public void MonitorLogFile()
        {
            _logger.LogInformation("Start Monitor Log File.");
            _service.ReadLog();
        }
    }
}
