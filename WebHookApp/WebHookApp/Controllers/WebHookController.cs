using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using WebHookApp.Hubs;
using WebHookApp.Models;
using WebHookApp.Services;

namespace WebHookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {

        private readonly IWebHookService _webHookService;

        private readonly IHubContext<webHookHub> _hubContext;

        public WebHookController(IWebHookService webHookService,IHubContext<webHookHub> hubContext)
        {
            _webHookService = webHookService;
            _hubContext = hubContext;
        }
        [HttpGet]
        public async Task<IActionResult> generateUrl()
        {
            try
            {
                var scheme = Request.Scheme;
                var host = Request.Host.ToString();

                var url = await _webHookService.SaveAndGenerateUrl(scheme, host);
                if (url != null)
                {
                    return Ok(new ResponseModel
                    {
                        statusCode = 200,
                        message = "your url retrieved",
                        data = url,
                        isSuccess = true
                    });
                } else
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "url not found",
                        data = "no data",
                        isSuccess = false
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.InnerException?.Message ?? ex.Message,
                    isSuccess = false
                });
            }
        }
        [Consumes("multipart/form-data", "application/json", "application/x-www-form-urlencoded")]
        [HttpDelete("receive/{urlId}")]
        [HttpPost("receive/{urlId}")]
        [HttpPatch("receive/{urlId}")]
        [HttpPut("receive/{urlId}")]
        [HttpOptions("receive/{urlId}")]

        public async Task<IActionResult> ReceiveUrl(Guid urlId, IFormFile? file = null)
        {
            var request = HttpContext.Request;
            try
            {
                if (request != null)
                {
                    var path = request.Path;
                    var method = request.Method;
                    string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    var userAgent = request.Headers["User-Agent"].ToString();
                    var header = string.Join("; ", request.Headers.Select(h => $"{h.Key}:{h.Value}"));
                    string body = method != HttpMethods.Options ? await new StreamReader(request.Body).ReadToEndAsync() : string.Empty;


                    var queryParams = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
                    var queryParamsJson = JsonConvert.SerializeObject(queryParams);

                   

                    var hookRequest = await _webHookService.SaveWebHookRequest(urlId, path, method, ipAddress, userAgent, header, body, queryParamsJson, file);

                    _hubContext.Clients.All.SendAsync("ReceivedWebHubRequest", hookRequest);
                        

                        
                    return Ok(new ResponseModel
                    {
                        statusCode = 200,
                        message = "your webhook request",
                        data = hookRequest,
                        isSuccess = true
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "not found",
                        data = "not data",
                        isSuccess = false
                    });
                }
            } catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.InnerException?.Message ?? ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpGet("receive/{urlId}")]
        [HttpHead("receive/{urlId}")]
        public async Task<IActionResult> ReceivedUrl(Guid urlId)
        {
            var request = HttpContext.Request;
            try
            {
                if (request != null)
                {
                    var path = request.Path;
                    var method = request.Method;
                    string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                    var userAgent = request.Headers["User-Agent"].ToString();
                    var header = string.Join("; ", request.Headers.Select(h => $"{h.Key}:{h.Value}"));
                    var body = await new StreamReader(request.Body).ReadToEndAsync();

                    var queryParams = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
                    var queryParamsJson = JsonConvert.SerializeObject(queryParams);


                    var hookRequest = await _webHookService.SaveWebHookRequest(urlId, path, method, ipAddress, userAgent, header, body, queryParamsJson,null);

                    _hubContext.Clients.All.SendAsync("ReceivedWebHubRequest", hookRequest);

                    return Ok(new ResponseModel
                    {
                        statusCode = 200,
                        message = "your webhook request",
                        data = hookRequest,
                        isSuccess = true
                    });
                }
                else
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "not found",
                        data = "not data",
                        isSuccess = false
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.InnerException?.Message ?? ex.Message,
                    isSuccess = false
                });
            }
        }

        [HttpGet("requests")]
        public async Task<IActionResult> getRequests()
        {
            try
            {
                var requests = await _webHookService.getRequest();

                if (requests == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "not found",
                        data = "no data",
                        isSuccess = false
                    });
                }
                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "your requests are here",
                    data = requests,
                    isSuccess = true
                });
            } catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.InnerException?.Message ?? ex.Message,
                    isSuccess = false
                });
            }
        }
        [HttpGet("getRequestById/{urlId}")]
        public async Task<IActionResult> searchRequest(Guid urlId)
        {
            try
            {
                var list = await _webHookService.searchRequest(urlId);

                if(list == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode = 404,
                        message = "no request retrieved",
                        data="no data",
                        isSuccess = false
                    });
                }
                return Ok(new ResponseModel
                {
                    statusCode = 200,
                    message = "your response",
                    data = list,
                    isSuccess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    statusCode = 500,
                    message = "Internal server error",
                    data = ex.InnerException?.Message ?? ex.Message,
                    isSuccess = false
                });
            }
          
        }
    }
}
