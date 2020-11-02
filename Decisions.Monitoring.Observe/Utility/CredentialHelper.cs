using Decisions.DesignerRepository.Client.Service;
using Decisions.Monitoring.Observe.Data;
using System;

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
                    BaseUrl = settings.BaseUrl,
                    Auth = GetAuthValueForHeader(settings.AccountNumber, settings.MetricsToken)
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
                    BaseUrl = settings.BaseUrl,
                    Auth = GetAuthValueForHeader(settings.AccountNumber, settings.LogToken)
                };
            }
        }

        private static string GetAuthValueForHeader(string accountNumber, string token)
        {
            if (String.IsNullOrEmpty(accountNumber) || String.IsNullOrEmpty(token)) return "";
            return accountNumber + ' ' + token;
        }
    }
}