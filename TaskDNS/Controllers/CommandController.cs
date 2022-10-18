﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("Close_CMD")]
        public async Task CloseCMD()
        {
            await cmd.CloseProcess();
        }

        [HttpGet]
        [Route("Get_Path")]
        public string GetPath()
        {
            return cmd.GetPath();
        }
        [HttpGet]
        [Route("Get_ArrayChildPath")]
        public JsonResult GetArrayChildPath()
        {
            return Json(cmd.GetChildPath());
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