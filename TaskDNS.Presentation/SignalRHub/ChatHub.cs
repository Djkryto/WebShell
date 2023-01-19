using Microsoft.AspNetCore.Authorization;
using TaskDNS.Application.Processes;
using Microsoft.AspNetCore.SignalR;
using TaskDNS.Domain.Interface;
using TaskDNS.Database.Model;
using TaskDNS.Network.Dto;

namespace TaskDNS.Network.SignalRHub
{
    /// <summary>
    /// Общий узел пользователей.
    /// </summary>
    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, CMDManager> _userCMD = new Dictionary<string, CMDManager>();
        private readonly ICommandRepository _commandRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        public ChatHub(ICommandRepository commandRepository) => _commandRepository = commandRepository;

        /// <summary>
        /// Добавление пользователя в словарь при подключении.
        /// </summary>
        public override Task OnConnectedAsync()
        {
            _userCMD.Add(Context.ConnectionId, new CMDManager(Context.ConnectionId));
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Удаления пользователя из словаря при подключении.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connectionId = Context.ConnectionId;

            _userCMD[connectionId].Dispose();
            _userCMD.Remove(connectionId);
        }

        /// <summary>
        /// Отправка комманды в консоль.
        /// </summary>
        public async Task AddCommand(string commandFromClient)
        {
            _userCMD[Context.ConnectionId].Write(commandFromClient);
            var data = new Command() { IdUserInHub = Context.ConnectionId, TextCommand = commandFromClient };

            await AddHistoryCommandUser(data);
        }

        /// <summary>
        /// Остановка выполнения комманды.
        /// </summary>
        public async Task Stop() => await _userCMD[Context.ConnectionId].StopAsync();

        /// <summary>
        /// Отправка истории пользователю.
        /// </summary>
        public async Task<ConsoleDataDto> GetConsoleData()
        {
            var commands = await _commandRepository.GetHistoryUserAsync(Context.ConnectionId);

            return new() { 
                Directory = _userCMD[Context.ConnectionId].GetDirectory(),
                SubDirectory = _userCMD[Context.ConnectionId].GetDirectories(),
                History = commands.Select(x => new CommandDto(x)).ToArray()
            };
        }

        private async Task AddHistoryCommandUser(Command command)
        {
            await _commandRepository.AddAsync(command);
            await _commandRepository.SaveAsync();
        }

        private async Task RemoveHistoryCommandUser(string connectionId)
        {
            await _commandRepository.RemoveAsync(connectionId);
            await _commandRepository.SaveAsync();
        }
    }
}