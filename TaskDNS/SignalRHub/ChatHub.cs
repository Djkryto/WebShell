using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskDNS.Channels;
using TaskDNS.Controllers;

namespace TaskDNS.SignalRHub
{
    /// <summary>
    /// Общий узел пользователей.
    /// </summary>
    public class ChatHub : Hub {}
}
