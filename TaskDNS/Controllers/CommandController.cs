using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using static System.Net.Mime.MediaTypeNames;

namespace TaskDNS.Controllers
{
    [Route("Server")]
    public class CommandController : Controller
    {

        private string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private string nameFile = "\\command.txt";

        IRepositoryCommand dbCommand;
        public CommandController(CommandContext context)
        {
            dbCommand = new SQLCommand(context);
        }

        [HttpPost]
        [Route("Add")]
        public JsonResult AddCommand([FromBody]Command command)
        {
            Process process = new Process();

            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = @"/C " + command.TextCommand;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();

            WriteFile(process.StandardOutput.ReadToEnd());

            process.WaitForExit();
            //////////////////////////////
            dbCommand.Add(command);
            dbCommand.Save();

            return Json(ReadFile());
        }

        [HttpGet]
        [Route("History")]
        public JsonResult History()
        {
            Command[] history = new Command[2];
            history = dbCommand.AllHistory().ToArray();
            return Json(history);
        }


        [HttpGet]
        [Route("Output")]
        public JsonResult OutputCommand()
        {
            return Json(ReadFile());
        }


        private void WriteFile(string text)
        {
            StreamWriter sw = new StreamWriter(appPath + nameFile);

            sw.WriteLine(text);
            sw.Close();
        }

        private Command ReadFile()
        {
            StreamReader sw = new StreamReader(appPath + nameFile);
            Command command = new Command(0,"Data", sw.ReadToEnd());
            sw.Close();
            return command;
        }
    }
}
