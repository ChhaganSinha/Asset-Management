using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace AssetManagement.Client.Client
{
    public class BaseHttpClient
    {
        public BaseHttpClient(ILogger logger, HttpClient httpClient)
        {
            Logger = logger;
            HttpClient = httpClient;
        }

        public ILogger Logger { get; }
        public HttpClient HttpClient { get; }

        protected async Task<T?> ReadFromJsonAsyncSafe<T>(HttpResponseMessage response)
        {
            if (response.Content == null)
                return default;

            if (response.Content.Headers?.ContentLength == 0)
                return default;

            if (response.Content.Headers?.ContentType == null)
            {
                var raw = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(raw))
                    return default;
                return JsonSerializer.Deserialize<T>(raw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}
