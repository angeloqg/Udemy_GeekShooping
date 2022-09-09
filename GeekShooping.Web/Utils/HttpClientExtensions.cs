using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShooping.Web.Utils
{
    public static class HttpClientExtensions
    {
        public static MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json");

        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw
                    new ApplicationException(
                        $"Somenthing went wrong calling the API:" +
                        $"{response.ReasonPhrase}");

            string dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(
                json: dataAsString,
                options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpClient.PutAsync(url, content);
        }

        public static List<T>? DesserializationDataRange<T>(object value)
        {
            string json = JsonSerializer.Serialize(value);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(json: json, options: options);
        }

        public static T? Desserialization<T>(object value)
        {
            string json = JsonSerializer.Serialize(value);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<T>(json: json, options: options);
        }
    }
}
