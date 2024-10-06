using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Extensions
{
    internal static class HttpContentExtensions
    {
        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        internal static async Task<T> GetAsync<T>(this HttpContent content)
        {
            var json = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }
    }
}
