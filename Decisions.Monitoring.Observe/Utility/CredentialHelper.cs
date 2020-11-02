using Decisions.Monitoring.Observe.Data;

namespace Decisions.Monitoring.Observe.Utility
{
    internal static class CredentialHelper
    {
        public static ObserveCredential MetricsCredentials
        {
            get
            {
                var settings = ObserveSettings.Instance();
                return new ObserveCredential
                {
                    BaseUrl = settings.MetricsBaseUrl,
                    Token = settings.MetricsToken ?? ""
                };
            }
        }

        public static ObserveCredential LogCredentials
        {
            get
            {
                var settings = ObserveSettings.Instance();
                return new ObserveCredential
                {
                    BaseUrl = settings.LogBaseUrl,
                    Token = settings.LogToken ?? ""
                };
            }
        }
    }
}