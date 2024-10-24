using Microsoft.AspNetCore.SignalR;
using WebHookApp.Models;

namespace WebHookApp.Hubs
{
    public class webHookHub:Hub
    {
        public async Task sendRequestUpdate(WebHookRequest request)
        {
            await Clients.All.SendAsync("ReceivedWebHubRequest", request);
        }
    }
}
