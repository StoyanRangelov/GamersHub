namespace GamersHub.Web.Hubs
{
    using System.Threading.Tasks;

    using GamersHub.Web.ViewModels.Home;
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
