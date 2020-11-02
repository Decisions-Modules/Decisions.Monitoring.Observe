using DecisionsFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Observe.Data
{
    public class ObserveLogData
    {
        [JsonIgnore]
        private LogData source;
        [JsonIgnore]
        private string basePortalUrlName;
        [JsonIgnore]
        private string hostName;

        public ObserveLogData(LogData source, string basePortalUrlName, string hostName)
        {
            this.source = source;
            this.basePortalUrlName = basePortalUrlName;
            this.hostName = hostName;
        }

        public string BasePortalUrlName { get => basePortalUrlName; }
        public string HostName { get => hostName; }

        public DateTime TimeStamp { get => source.TimeStamp; }
        public string Level { get => source.LevelName; }
        public string Category { get => source.Category; }
        public string Message { get => source.Message; }
        public int ThreadId { get => source.ThreadId; }
        public string Exception { get => source.Exception?.ToString(); }
        public string SessionID { get => source.SessionID; }
        public string Activity { get => source.Activity; }
    }
}
