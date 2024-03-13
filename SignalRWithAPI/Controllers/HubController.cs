using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRWithAPI.Hubs;

namespace SignalRWithAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HubController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public HubController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;   
        }

        [HttpGet]
        public string HubMessage(string user,string message)
        {
            _hubContext.Clients.All.SendAsync("ReceiveMessage", user,message);
            return "message Successfully send";
        }

        [HttpPost]
        public string  HubMessageToUser(string user, string connectionId, string message)
        {
            _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
            return $"Message successfully sent from one user to another user,{ connectionId}";
        }

    }
}

