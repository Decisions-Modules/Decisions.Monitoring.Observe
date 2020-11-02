using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Decisions.Monitoring.Observe.Data;
using Newtonsoft.Json;

namespace Decisions.Monitoring.Observe.Utility
{
    public static partial class ObserveApi
    {
        private static HttpClient GetClient(ObserveCredential credential)
        {
            var httpClient = new HttpClient {BaseAddress = new Uri(credential.BaseUrl) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.Auth);

            return httpClient;
        }

        private static string ParseRequestContent<T>(T content)
        {
            var data = JsonConvert.SerializeObject(content);
            return data;
        }

        private static string CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }

            return null;
        }

        public static bool PostRequest<R, T>(ObserveCredential connection, string requestUri, params T[] content) where R : class, new()
        {
            var data = ParseRequestContent(content);

            var contentStr = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            var response = GetClient(connection).PostAsync(requestUri, contentStr).Result;

            return CheckResponse(response)==null;
        }
    }
}