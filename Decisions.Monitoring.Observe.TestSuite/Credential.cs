using Decisions.Monitoring.Observe.Data;

namespace Decisions.Monitoring.Observe.TestSuit
{
    static class Credential
    {
        public static ObserveCredential GetCredential()
        {
            return new ObserveCredential()
            {
                BaseUrl = "https://collect.observeinc.com/v1/observations/test",
                PartialUrl = "metrics",
                Auth = "put you account Id and token here"
            };
        }
    }
}
