using System;
using System.Linq;
using Decisions.Monitoring.Observe.Data;
using DecisionsFramework;

namespace Decisions.Monitoring.Observe.Utility
{
    public static partial class ObserveApi
    {
        private static readonly string logType = "DecisionsLog";
        private static readonly string metricsType = "DecisionsMetrics";

        public static bool SendLog(params ObserveLogData[] logs)
        {
            ObserveCredential connection = CredentialHelper.LogCredentials;
            if (connection == null || string.IsNullOrEmpty(connection.Token) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;
            try
            {
                var resp = PostRequest<Object, ObserveLogData>(connection, "", logs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendMetrics( params ObserveMetricsData[] metris)
        {
            ObserveCredential connection = CredentialHelper.MetricsCredentials;
            if (connection == null || string.IsNullOrEmpty(connection.Token) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;

            try
            {
                var resp = PostRequest<object, ObserveMetricsData>(connection, "", metris);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}