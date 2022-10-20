using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using TaskDNS.Channels.Interface;
using TaskDNS.App.Processes;

namespace TaskDNS.Controllers
{
    /// <summary>
    /// Класс отвечающий за выполнение комманд.
    /// </summary>
    [Route("Server")]
    public class CommandController : Controller
    {
        private ICommandRepository dbCommand;
        private CMD cmd;

        /// <summary>
        /// Значения в конструкторе необходимы для запуска процесса(cmd.exe) и обращение в БД. 
        /// </summary>
        /// <param name="context">Обращение в базу данных.</param>
        /// <param name="cmd">Класс процесса cmd.exe</param>
        public CommandController(CommandContext context,CMD cmd)
        {
            dbCommand = new CommandRepostiory(context);
            this.cmd = cmd;
        }

        /// <summary>
        /// Получение команды от клиента.
        /// </summary>
        /// <param name="commandFromClient">Комманда пришедшая от клиента</param>
        /// <returns></returns>
        [HttpPost("add")]
        public void AddCommand([FromBody]Command commandFromClient)
        {
            cmd.Write(commandFromClient.TextCommand);
            //////////////////////////////
            dbCommand.Add(commandFromClient);
            dbCommand.Save();
        }
        /// <summary>
        /// Внешний метод закрытия консоли(cmd.exe).
        /// </summary>
        [HttpPost("stop")]
        public async Task StopAsync()
        {
           await cmd.StopAsync();
        }
        /// <summary>
        /// Внешний метод отправки текущей директории клиениту.
        /// </summary>
        [HttpGet("getDirectory")]
        public string GetDirectory()
        {
            return cmd.GetDirectory();
        }
        /// <summary>
        /// Внешний метод отправки всех директорий клиенту относительно текущей директории.
        /// </summary>
        [HttpGet("getDirectories")]
        public JsonResult GetDirectories()
        {
            return Json(cmd.GetDirectories());
        }
        /// <summary>
        /// Метод для отправки истории комманд клиенту.
        /// </summary>
        [HttpGet("getHistory")]
        public JsonResult GetHistory()
        {
            var history = new Command[2]; 
            history = dbCommand.AllHistory().ToArray();
            return Json(history);
        }
    }
}
