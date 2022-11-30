using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using TaskDNS.App.Processes;
using TaskDNS.Models.Dto;
using TaskDNS.Models.SQLServer.Repository;

namespace TaskDNS.Controllers
{
    /// <summary>
    /// Класс отвечающий за взаимодействие с базой данных и процессом cmd.exe.
    /// </summary>
    [Authorize]
    [Route("command")]
    public class CommandController : Controller
    {
        private readonly ICommandRepository _bdCommand;
        private readonly CMD _cmd;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="context">Взаимодействие с базой данных</param>
        /// <param name="cmd">Взаимодействие клиента с процессом cmd.exe</param>
        public CommandController(SQLContext context,CMD cmd)
        {
            _bdCommand = new CommandRepository(context);
            _cmd = cmd;
        }

        /// <summary>
        /// Получение команды от клиента.
        /// </summary>
        /// <param name="commandFromClient"></param>
        [HttpPost("add")]
        public void AddCommand([FromBody]Command commandFromClient)
        {
            _cmd.Write(commandFromClient.TextCommand);

            _bdCommand.Add(commandFromClient);
            _bdCommand.Save();
        }

        /// <summary>
        /// Закрытие консоли(cmd.exe).
        /// </summary>
        [HttpPost("stop")]
        public async Task Stop()
        {
           await _cmd.StopAsync();
        }

        /// <summary>
        /// Отправка текущей директории клиениту.
        /// </summary>
        [HttpGet("getDirectory")]
        public string GetDirectory()
        {
            return _cmd.GetDirectory();
        }

        /// <summary>
        /// Отправка всех директорий клиенту относительно текущей директории.
        /// </summary>
        [HttpGet("getDirectories")]
        public JsonResult GetDirectories()
        {
            return Json(_cmd.GetDirectories());
        }

        /// <summary>
        /// Отправка истории комманд клиенту.
        /// </summary>
        [HttpGet("getHistory")]
        public JsonResult GetHistory()
        {
            var history = _bdCommand.GetHistory().ToArray();
            var historyDto = history.Select(x => new CommandDto(x));

            return Json(historyDto);
        }
    }
}
