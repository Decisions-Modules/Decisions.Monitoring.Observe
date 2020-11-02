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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.Token);

            return httpClient;
        }

        private static string ParseRequestContent<T>(T content)
        {
            var data = JsonConvert.SerializeObject(content);
            return data;
        }

        private static ObserveErrorResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var err = ParseResponse<ObserveErrorResponse>(response);
                return err;
            }

            ;
            return null;
        }

        private static R ParseResponse<R>(HttpResponseMessage response) where R : new()
        {
            var responseString = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<R>(responseString);

            return result;
        }

        private static R PostRequest<R, T>(ObserveCredential connection, string requestUri, T[] content) where R : class, new()
        {
            var date = ParseRequestContent(content);
           /* var data = new StringBuilder();

            foreach (var item in content)
            {
                if (data.Length > 0) data.Append("\n");
                data.Append(ParseRequestContent(item));
            }*/

            var contentStr = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            var response = GetClient(connection).PostAsync(requestUri, contentStr).Result;

            CheckResponse(response);
            return ParseResponse<R>(response);
        }
    }
}