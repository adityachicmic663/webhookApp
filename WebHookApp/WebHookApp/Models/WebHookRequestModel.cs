using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebHookApp.Models
{
    public class WebHookRequestModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int requestId { get; set; }
        [Required]
        public string path { get; set; }
        [Required]
        public string method { get; set; }

        public DateTime timeStamp { get; set; }

        public string ipAddress { get; set; }

        public string userAgent { get; set; }

        public string headers { get; set; }

        public string body { get; set; }

        public string queryParams { get; set; }

        public string? filePath {  get; set; }

        public Guid urlId { get; set; }

        public WebHookUrl url { get; set; }
    }
}
