using Newtonsoft.Json;

namespace Decisions.Monitoring.Observe.Data
{
    public class ObserveMetricsData
    {
        [JsonProperty(PropertyName = "metrics")]
        public ObserveMetrics Metrics { get; set; }

        [JsonProperty(PropertyName = "dimensions")]
        public ObserveDimensions Dimensions { get; set; }
    }

    public class ObserveMetrics
    {
        public int DetailCount { get; set; }
    }

    public class ObserveDimensions
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string HostName { get; set; }
        public string BasePortalUrlName { get; set; }
        public string DecisionsVersion { get; set; }
    }
}