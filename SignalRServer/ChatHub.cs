using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRServer
{
    public class ChatHub : Hub
    {
        public async Task SayHello()
        {
            await Clients.All.SendAsync("Hello");
        }
    }
}