namespace WebHookApp.Client.Models
{
    public class WebHookRequestModel
    {
        public string path { get; set; }
        public string method { get; set; }
        public string ipAddress { get; set; }
        public string userAgent { get; set; }
        public string headers { get; set; }
        public string body { get; set; }
        public string queryParams { get; set; }
        public DateTime timeStamp { get; set; }
        public List<string> files { get; set; } = new List<string>();
    }
}
