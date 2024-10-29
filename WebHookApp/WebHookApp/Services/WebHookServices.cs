using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using WebHookApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebHookApp.Services
{
    public class WebHookServices:IWebHookService
    {
        private readonly webHookDataContext _dataContext;
        private readonly string _uploadPath;

        public WebHookServices(webHookDataContext dataContext,IConfiguration configuration)
        {
            _dataContext = dataContext;
            _uploadPath = configuration.GetValue<string>("UploadPath");
        }
        public async Task<WebHookUrlResponse> SaveAndGenerateUrl(string scheme,string host)
        {
            Guid urlId= Guid.NewGuid();
            var webHookUrl = $"{scheme}://{host}/api/webhook/receive/{urlId}";

            var webHook = new WebHookUrl
            {
                urlId = urlId,
                url = webHookUrl,
                generatedAt = DateTime.Now
            };
            var response = new WebHookUrlResponse
            {
                urlId = webHook.urlId,
                url = webHook.url,
                generatedAt = webHook.generatedAt
            };
           
            await _dataContext.Urls.AddAsync(webHook); 
             await _dataContext.SaveChangesAsync();
            return response;  
        }

        public async Task<WebHookRequest> SaveWebHookRequest(Guid urlId, string path, string method, string ipAddress, string userAgent, string header, string body, string queryParams, IFormFile? file = null)
        {
            string filePath = null;
            if (file != null && file.Length > 0) {
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                filePath = Path.Combine(_uploadPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        
            var webhookRequest = new WebHookRequestModel
            {
                path = path,
                method = method,
                ipAddress = ipAddress,
                userAgent = userAgent,
                headers = header,
                body = body,
                queryParams = queryParams,
                timeStamp = DateTime.Now,
                filePath = filePath,
                urlId= urlId
            };

            await _dataContext.requests.AddAsync(webhookRequest);
            await _dataContext.SaveChangesAsync();

            var request = new WebHookRequest
            {
                requestId = webhookRequest.requestId,
                path = path,
                method = method,
                ipAddress = ipAddress,
                userAgent = userAgent,
                headers = header,
                body = body,
                queryParams = queryParams,
                timeStamp = webhookRequest.timeStamp,
                filePath = filePath
            };
            return request;
        }

       public async Task<List<WebHookRequest>> getRequest()
        {
            var list = await _dataContext.requests.OrderByDescending(x => x.timeStamp).ToListAsync();

            var responseList=new List<WebHookRequest>();
            foreach (var item in list)
            {
                var response = new WebHookRequest
                {
                    requestId= item.requestId,
                    path = item.path,
                    method = item.method,
                    ipAddress = item.ipAddress,
                    userAgent = item.userAgent,
                    headers = item.headers,
                    body = item.body,
                    queryParams = item.queryParams,
                    timeStamp = item.timeStamp,
                    filePath = item.filePath
                };
                responseList.Add(response);
            }
            return responseList;
        }

        public async Task<List<WebHookRequest>> searchRequest(Guid urlId)
        {
            var request = await _dataContext.requests.Where(x=>x.urlId == urlId).ToListAsync();

            var responselist=new List<WebHookRequest>();

            foreach(var item in request)
            {
                var response = new WebHookRequest()
                {
                    requestId = item.requestId,
                    path = item.path,
                    method = item.method,
                    ipAddress = item.ipAddress,
                    userAgent = item.userAgent,
                    headers = item.headers,
                    body = item.body,
                    queryParams = item.queryParams,
                    timeStamp = item.timeStamp,
                    filePath = item.filePath
                };
                responselist.Add(response);
            }
            return responselist;
            
        }
    }
}
