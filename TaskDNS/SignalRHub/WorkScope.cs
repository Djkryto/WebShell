using Microsoft.AspNetCore.SignalR;
using TaskDNS.Channels;

namespace TaskDNS.SignalRHub
{ 
    /// <summary>
    /// Класс по создание и настройка узла.
    /// </summary>
    public class WorkScope : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        /// <summary>
        /// Создание рабочего пространства.
        /// </summary>
        /// <param name="serviceScopeFactory">Сервис рабочего пространства.</param>
        public WorkScope(IServiceScopeFactory serviceScopeFactory)
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
