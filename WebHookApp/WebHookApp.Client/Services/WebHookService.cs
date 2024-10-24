using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebHookApp.Client.Models;

namespace WebHookApp.Client.Services
{
    public class WebHookService
    {
        private readonly HttpClient _httpClient;

        public WebHookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WebHookRequest>> GetWebHookRequestsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseModel>("api/webhook");
            return response != null && response.isSuccess ? response.data as List<WebHookRequest> : new List<WebHookRequest>();
        }

        public async Task<string> GenerateWebHookUrlAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("api/webhook/generateUrl", new { });
            var result = await response.Content.ReadFromJsonAsync<ResponseModel>();
            return result != null && result.isSuccess ? result.data.ToString() : null;
        }

        public async Task<WebHookRequest> GetWebHookRequestByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<WebHookRequest>($"api/webhook/{id}");
        }
    }
}
