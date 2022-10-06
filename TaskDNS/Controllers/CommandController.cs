using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using static System.Net.Mime.MediaTypeNames;
using TaskDNS.App;

namespace TaskDNS.Controllers
{
    [Route("Server")]
    public class CommandController : Controller
    {
        IRepositoryCommand dbCommand;

        CMD cmd = new CMD();

        public CommandController(CommandContext context)
        {
            dbCommand = new SQLCommand(context);
        }

        [HttpPost]
        [Route("Add")]
        public JsonResult AddCommand([FromBody]Command command)
        {
            cmd.Write(command.TextCommand);
            cmd.Start();
            //////////////////////////////
            dbCommand.Add(command);
            dbCommand.Save();
            //////////////////////////////
            return Json(null);
        }

        [HttpGet]
        [Route("Close_CMD")]
        public Task CloseCMD()
        {
            cmd.CloseProcess();
            return Task.CompletedTask;
        }

        [HttpGet]
        [Route("History")]
        public JsonResult History()
        {
            Command[] history = new Command[2]; 
            history = dbCommand.AllHistory().ToArray();
            return Json(history);
        }
    }
}
