using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Reflection.PortableExecutable;
using WebHookApp.Models;
using WebHookApp.Services;

namespace WebHookApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebHookController : ControllerBase
    {

        private readonly IWebHookService _webHookService;

        public WebHookController(IWebHookService webHookService)
        {
            _webHookService = webHookService;
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

        [HttpDelete("receive/{urlId}")]
        [HttpGet("receive/{urlId}")]
        [HttpPost("receive/{urlId}")]
        [HttpPatch("receive/{urlId}")]
        [HttpPut("receive/{urlId}")]
        [HttpHead("receive/{urlId}")]
        [HttpOptions("receive/{urlId}")]
       
        public async Task<IActionResult> ReceiveUrl(Guid urlId,IFormFile? file=null)
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
                    var header = string.Join("; ", request.Headers.Select(h=>$"{h.Key}:{h.Value}"));
                    var body = await new StreamReader(request.Body).ReadToEndAsync();
                    var queryParams = string.Join("&", request.Query.Select(q => $"{q.Key}={q.Value}"));

                    var hookRequest = await _webHookService.SaveWebHookRequest(urlId,path, method, ipAddress, userAgent, header, body, queryParams,file);


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
            }catch (Exception ex)
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
                var requests = _webHookService.getRequest();

                if(requests == null)
                {
                    return NotFound(new ResponseModel
                    {
                        statusCode=404,
                        message="not found",
                        data="no data",
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
            }catch(Exception ex)
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
