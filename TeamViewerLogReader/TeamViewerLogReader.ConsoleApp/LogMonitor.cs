using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Service.Interfaces;

namespace TeamViewerLogReader.ConsoleApp
{
    
        public class LogMonitor
        {
        private readonly ILogRederService _service;

            public LogMonitor(ILogRederService service)
            {
                _service = service;
            }

            public void MonitorLogFile()
            {
                _service.ReadLog();
            }
        }
}
