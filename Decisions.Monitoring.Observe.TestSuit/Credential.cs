using Decisions.Monitoring.Observe.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Observe.TestSuit
{
    class Credential
    {
        public static ObserveCredential GetCredentinal()
        {
            return new ObserveCredential()
            {
                BaseUrl = "https://collect.observeinc.com/v1/observations/test",
                Auth = "put you account Id and token here"
            };
        }
    }
}
