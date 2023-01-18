using Microsoft.Extensions.DependencyInjection;
using TaskDNS.Application.Processes.Channels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using TaskDNS.Network.Dto;

namespace TaskDNS.Network.SignalRHub
{
    /// <summary>
    /// Класс по отправке сообщений клиентам.
    /// </summary>
    public class MessageSender : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// .ctor
        /// </summary>
        public MessageSender(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                await foreach (var dataOutput in CommandChannelProvider.CommandResultChannel.Reader.ReadAllAsync())
                {
                    if (string.IsNullOrEmpty(dataOutput.ConnectionId))
                        continue;

                    var score = _serviceScopeFactory.CreateScope();
                    var chatHub = score.ServiceProvider.GetService<IHubContext<ChatHub>>();
                    var output = new OutputConsoleDto(dataOutput.Output, Convert.ToByte(dataOutput.Status));

                    await chatHub.Clients.Client(dataOutput.ConnectionId).SendAsync("Send", output);
                }
            });
        }
    }
}