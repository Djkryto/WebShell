using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using TaskDNS.App;
using TaskDNS.Channels.Interface;
using TaskDNS.App.Processes;
using TaskDNS.Models.Dto;

namespace TaskDNS.Controllers
{
    /// <summary>
    /// Класс отвечающий за взаимодействие с базой данных и процессом cmd.exe.
    /// </summary>
    [Route("Server")]
    public class CommandController : Controller
    {
        private ICommandRepository BdCommand { get; }
        private CMD Cmd { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context">Взаимодействие с базой данных</param>
        /// <param name="cmd">Взаимодействие клиента с процессом cmd.exe</param>
        public CommandController(CommandContext context,CMD cmd)
        {
            BdCommand = new CommandRepostiory(context);
            this.Cmd = cmd;
        }

        /// <summary>
        /// Получение команды от клиента.
        /// </summary>
        /// <param name="commandFromClient"></param>
        [HttpPost("add")]
        public async Task<JsonResult> AddCommand([FromBody]Command commandFromClient)
        {
            Cmd.Write(commandFromClient.TextCommand);
            BdCommand.Add(commandFromClient);
            BdCommand.Save();
            //////////////////////////////
            return Json(null);
        }

        /// <summary>
        /// Внешний метод закрытия консоли(cmd.exe).
        /// </summary>
        [HttpGet("stop")]
        public async Task Stop()
        {
           await Cmd.StopAsync();
        }

        /// <summary>
        /// Внешний метод отправки текущей директории клиениту.
        /// </summary>
        [HttpGet("getDirectory")]
        public string GetDirectory()
        {
            return Cmd.GetDirectory();
        }

        /// <summary>
        /// Внешний метод отправки всех директорий клиенту относительно текущей директории.
        /// </summary>
        [HttpGet("getDirectories")]
        public JsonResult GetDirectories()
        {
            return Json(Cmd.GetDirectories());
        }

        /// <summary>
        /// Метод для отправки истории комманд клиенту.
        /// </summary>
        [HttpGet("getHistory")]
        public JsonResult GetHistory()
        {
            var history = BdCommand.AllHistory().ToArray();
            var historyDto = history.Select(x => new CommandDto(x));
            return Json(historyDto);
        }
    }
}
