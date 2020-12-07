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
            ObserveCredential connection = CredentialHelper.GetCredentials;
            connection.PartialUrl = "logs";
            if (connection == null || string.IsNullOrEmpty(connection.Auth) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;
            try
            {
                var res = PostRequest<Object, ObserveLogData>(connection, "", logs);
                return res;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendMetrics( params ObserveMetricsData[] metrics)
        {
            ObserveCredential connection = CredentialHelper.GetCredentials;
            connection.PartialUrl = "metrics";
            if (connection == null || string.IsNullOrEmpty(connection.Auth) ||
                string.IsNullOrEmpty(connection.BaseUrl))
                return false;

            try
            {
                var res = PostRequest<object, ObserveMetricsData>(connection, "", metrics);
                return res;
            }
            catch
            {
                return false;
            }
        }

    }
}