namespace WebHookApp.Client.Models
{
    public class WebHookRequest
    {

        public int id {  get; set; }

        public string path {  get; set; }

        public string method { get; set; }  

        public DateTime timeStamp { get; set; } 
    }
}
