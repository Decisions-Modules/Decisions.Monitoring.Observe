using System;
using System.Collections.Generic;
using Decisions.Monitoring.Observe.Data;
using Decisions.Monitoring.Observe.Utility;
using DecisionsFramework;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.Utilities.Profiler;
using DecisionsFramework.Utilities.Profiler.Heartbeat;

namespace Decisions.Monitoring.Observe
{
    public class ObserveMetricsSummarizer : IProfilerSummaryWriter, IInitializable
    {
        public void Initialize()
        {
            ProfilerService.SummaryWriter = this;
        }

        public void WriteSummary(ProfileWriterData header, ProfileTimeSummary timeSummary)
        {
            // Do Nothing.  
        }
    }

    public class ObserveMetricsWriter : IProfilerDetailWriter, IInitializable
    {
        private readonly MetricsSendingThreadJob metricSendingJob = new MetricsSendingThreadJob();

        public void Initialize()
        {
            ProfilerService.DetailWriter = this;
        }

        public void WriteDetail(ProfileWriterData header, ProfilerDetail[] details, TimeSpan time)
        {
            if (header.type == ProfilerType.Usage && details != null && details.Length > 0)
            {
                var metrics = new List<ObserveMetricsData>(details.Length);
                foreach (var eachEntry in details) metrics.Add(CreateMetrics(header, eachEntry));
                metricSendingJob.AddItem(metrics.ToArray());
            }
        }

        public void WriteHeartbeatData(HeartbeatData heartbeat)
        {
            if (heartbeat != null)
            {
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Flow Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Flow Runs")
                        {
                            Count = heartbeat.NumberOfFlowStarts
                        }
                    }, TimeSpan.Zero);
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Rule Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Rule Runs")
                        {
                            Count = heartbeat.NumberOfRuleExecutions
                        }
                    }, TimeSpan.Zero);
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("API Calls", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "API Calls")
                        {
                            Count = heartbeat.NumberOfAPICalls
                        }
                    }, TimeSpan.Zero);
                ProfilerService.DetailWriter?.WriteDetail(
                    new ProfileWriterData("Job Runs", null, ProfilerType.Usage),
                    new[]
                    {
                        new ProfilerDetail(ProfilerDetailType.Info, "Job Runs")
                        {
                            Count = heartbeat.NumberOfJobStarts
                        }
                    }, TimeSpan.Zero);
            }
        }
        
        private ObserveMetricsData CreateMetrics(ProfileWriterData header, ProfilerDetail detail)
        {
            var settings = Settings.GetSettings();

            var metrics = new ObserveMetricsData
            {
                Metrics = new ObserveMetrics {DetailCount = detail.Count},
                Dimensions = new ObserveDimensions
                {
                    Name = header.Name,
                    Details = detail.Message,
                    HostName = Environment.MachineName,
                    BasePortalUrlName = settings.PortalBaseUrl,
                    DecisionsVersion = DecisionsVersion.VERSION
                }
            };
            return metrics;
        }
    }

    internal class MetricsSendingThreadJob : DataSendingThreadJob<ObserveMetricsData>
    {
        public MetricsSendingThreadJob() : base("Decisions.Observe metrics queue", TimeSpan.FromSeconds(10))
        {
        }

        protected override void SendData(ObserveMetricsData[] metrics)
        {
            if (!ObserveApi.SendMetrics(metrics))
                LogConstants.SYSTEM.Error("Decisions.Monitoring.Observe.io failed to send Metrics");
        }
    }
}