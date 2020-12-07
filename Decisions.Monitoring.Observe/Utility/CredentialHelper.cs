using Decisions.Monitoring.Observe.Data;
using System;

namespace Decisions.Monitoring.Observe.Utility
{
    internal static class CredentialHelper
    {
        public static ObserveCredential GetCredentials
        {
            get
            {
                var settings = ObserveSettings.Instance();
                return new ObserveCredential
                {
                    BaseUrl = settings.BaseUrl,
                    Auth = GetAuthValueForHeader(settings.AccountNumber, settings.AuthToken)
                };
            }
        }

        private static string GetAuthValueForHeader(string accountNumber, string token)
        {
            if (String.IsNullOrEmpty(accountNumber) || String.IsNullOrEmpty(token)) return "";
            return $"Bearer {accountNumber} {token}";
        }
    }
}