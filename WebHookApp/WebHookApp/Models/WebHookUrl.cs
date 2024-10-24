using System.ComponentModel.DataAnnotations;

namespace WebHookApp.Models
{
    public class WebHookUrl
    {
        [Key]
        [Required]
        public Guid urlId {  get; set; }    

        public string url { get; set; }

        public DateTime generatedAt { get; set; }

        public List<WebHookRequestModel> requests { get; set; } = new List<WebHookRequestModel>();
    }
}
