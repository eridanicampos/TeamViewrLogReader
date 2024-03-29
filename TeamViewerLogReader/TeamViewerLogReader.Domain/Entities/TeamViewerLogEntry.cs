﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Domain
{
    public class TeamViewerLogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public Guid UserTvLogId { get; set; }
        [ForeignKey("UserTvLogId")]
        public virtual UserTvLog UserTvLog { get; set; }

    }
}
