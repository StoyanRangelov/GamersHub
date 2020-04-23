using System.Threading.Tasks;
using GamersHub.Web.ViewModels.Home;

namespace GamersHub.Web.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ChatHub : Hub
    {
         public async Task Send(string message)
                {
                    await this.Clients.All.SendAsync(
                        "NewMessage",
                        new ChatMessageViewModel { User = this.Context.User.Identity.Name, Message = message, });
                }
    }
}