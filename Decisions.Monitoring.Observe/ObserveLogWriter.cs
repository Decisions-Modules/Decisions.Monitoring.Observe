using Decisions.Monitoring.Observe.Data;
using Decisions.Monitoring.Observe.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using System;

namespace Decisions.Monitoring.Observe
{
    public class ObserveLogWriter : ILogWriter, IInitializable
    {
        private readonly LogSendingThreadJob logSendingJob = new LogSendingThreadJob();

        public void Initialize()
        {
            Log.AddLogWriter(this);
        }

        public void Write(LogData log)
        {
            var moduleSettings = ObserveSettings.Instance();
            if ((moduleSettings.MinSendLogLevel == LogLevel.None) ||
                ((int)log.Level < (int)moduleSettings.MinSendLogLevel) )
                return;

            var decisionsSettings = Settings.GetSettings();
            ObserveLogData logData = new ObserveLogData(log, decisionsSettings.PortalBaseUrl, System.Environment.MachineName);
            logSendingJob.AddItem(logData);
        }
    }

    internal class LogSendingThreadJob : DataSendingThreadJob<ObserveLogData>
    {
        public LogSendingThreadJob() : base("Decisions.Observe log queue", TimeSpan.FromSeconds(10))
        {
        }

        protected override void SendData(ObserveLogData[] logs)
        {
            ObserveApi.SendLog(logs); // We ignore log error because we cannot write log-error to log
        }
    }
}