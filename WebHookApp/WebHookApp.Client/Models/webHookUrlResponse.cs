namespace WebHookApp.Client.Models
{
	public class webHookUrlResponse
	{
		public Guid urlId { get; set; }

		public string url { get; set; }

		public DateTime generatedAt { get; set; }
	}
}
