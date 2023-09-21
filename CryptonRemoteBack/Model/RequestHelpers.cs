using Newtonsoft.Json;
using System.Text;

namespace CryptonRemoteBack.Model
{
    public class RequestHelpers
    {
        internal static async Task<string?> PostMessageAsync(object message, string reqPath)
        {
            string messageJson = JsonConvert.SerializeObject(message);
            StringContent httpContent = new(messageJson,
                                            Encoding.UTF8,
                                            "application/json");

            using HttpClient httpClient = new();
            HttpResponseMessage httpResponse = await httpClient
                .PostAsync(reqPath, httpContent);

            return httpResponse.Content != null ? await httpResponse.Content.ReadAsStringAsync() : null;
        }

        internal static async Task<string?> GetMessageAsync(string reqPath)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage resp = await httpClient
                .GetAsync(reqPath);

            //object? result = JsonConvert.DeserializeObject<result>(body);
            return resp.Content != null ? await resp.Content.ReadAsStringAsync() : null;
        }
    }
}
