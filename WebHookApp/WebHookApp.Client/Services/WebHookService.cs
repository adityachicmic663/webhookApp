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

        public async Task<List<webHookRequest>> GetWebHookRequestsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseModel>("api/webhook");
            return response != null && response.isSuccess ? response.data as List<webHookRequest> : new List<webHookRequest>();
        }

        public async Task<string> GenerateWebHookUrlAsync()
        {
            var response = await _httpClient.PostAsJsonAsync("api/webhook/generateUrl", new { });
            var result = await response.Content.ReadFromJsonAsync<ResponseModel>();
            return result != null && result.isSuccess ? result.data.ToString() : null;
        }

        public async Task<webHookRequest> GetWebHookRequestByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<webHookRequest>($"api/webhook/{id}");
        }
    }
}
