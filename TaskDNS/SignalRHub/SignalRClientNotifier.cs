using Microsoft.AspNetCore.SignalR;
using TaskDNS.Channels;

namespace TaskDNS.SignalRHub
{ 
    /// <summary>
    /// Класс по отправке сообщений клиентам.
    /// </summary>
    public class SignalRClientNotifier : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        /// <summary>
        /// .ctor
        /// </summary>
        public SignalRClientNotifier(IServiceScopeFactory serviceScopeFactory)
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
