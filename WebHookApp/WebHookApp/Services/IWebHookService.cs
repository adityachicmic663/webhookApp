using WebHookApp.Models;

namespace WebHookApp.Services
{
    public interface IWebHookService
    {
        Task<WebHookUrlResponse> SaveAndGenerateUrl(string scheme, string host);
        Task<WebHookRequest> SaveWebHookRequest(Guid urlId,string path, string method, string ipAddress, string userAgent, string header, string body, string queryParams,IFormFile? file=null);

        Task<List<WebHookRequest>> getRequest();

        Task<List<WebHookRequest>> searchRequest(Guid urlId);

    }
}
