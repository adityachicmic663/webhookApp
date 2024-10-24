using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHookApp.Models
{
    public class WebHookRequest
    {
        public int requestId { get;set; }

        public string path { get; set; }

        public string method { get; set; }

        public DateTime timeStamp { get; set; } 

        public string ipAddress {  get; set; }

        public string userAgent {  get; set; }  

        public string headers {  get; set; }    

        public string body {  get; set; }   

        public string queryParams {  get; set; }

        public string? filePath { get; set; }


    }
}
