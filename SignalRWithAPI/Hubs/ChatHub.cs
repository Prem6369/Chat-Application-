using System;
using Microsoft.AspNetCore.SignalR;

namespace SignalRWithAPI.Hubs
{
    public class ChatHub : Hub
    {

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        public async Task SendMessageToAll(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);


        }

        public async Task SendMessageUser(string user,string connectionid, string message)
        {
            await Clients.Client(connectionid).SendAsync("ReceiveMessage", user,connectionid,message);   
        }
    }
}
