namespace WebHookApp.Models
{
    public class WebHookUrlResponse
    {
        public Guid urlId { get; set; }

        public string url { get; set; }

        public DateTime generatedAt { get; set; }
    }
}
