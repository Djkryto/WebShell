using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using TaskDNS.App;
using TaskDNS.Channels.Interface;

namespace TaskDNS.Controllers
{
    [Route("Server")]
    public class CommandController : Controller
    {
        public CommandController(CommandContext context,CMD cmd)
        {
            dbCommand = new SQLCommand(context);
            this.cmd = cmd;
        }

        IRepositoryCommand dbCommand;
        CMD cmd;

        /// <summary>
        /// Получение команды от клиента.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]
        public async Task<JsonResult> AddCommand([FromBody]Command command)
        {
            cmd.Write(command.TextCommand);
            //////////////////////////////
            dbCommand.Add(command);
            dbCommand.Save();
            //////////////////////////////
            return Json(null);
        }
        /// <summary>
        /// Внешний метод закрытия консоли(cmd.exe).
        /// </summary>
        [HttpGet]
        [Route("Stop")]
        public void Stop()
        {
            cmd.Stop();
        }
        /// <summary>
        /// Внешний метод отправки текущей директории клиениту.
        /// </summary>
        [HttpGet]
        [Route("Get_Directory")]
        public string GetPath()
        {
            return cmd.GetDirectory();
        }
        /// <summary>
        /// Внешний метод отправки всех директорий клиенту относительно текущей директории.
        /// </summary>
        [HttpGet]
        [Route("Get_Directories")]
        public JsonResult GetDirectories()
        {
            return Json(cmd.GetDirectories());
        }
        /// <summary>
        /// Метод для отправки истории комманд клиенту.
        /// </summary>
        [HttpGet]
        [Route("Get_History")]
        public JsonResult GetHistory()
        {
            Command[] history = new Command[2]; 
            history = dbCommand.AllHistory().ToArray();
            return Json(history);
        }
    }
}
