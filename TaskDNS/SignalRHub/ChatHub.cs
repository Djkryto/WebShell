using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using System.Threading.Tasks;
using TaskDNS.Channels;
using TaskDNS.Controllers;

namespace TaskDNS.SignalRHub
{
    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }

    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                await foreach (var output in ChannelProvider.CommandChannel.Reader.ReadAllAsync())
                {
                    var score = _serviceScopeFactory.CreateScope();
                    var chatHub = score.ServiceProvider.GetService<IHubContext<ChatHub>>();

                    await chatHub.Clients.All.SendAsync("Send", output);
                }
            });
        }
    }
}
