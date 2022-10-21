using TaskDNS.Controllers.Interface;
using Microsoft.EntityFrameworkCore;
using TaskDNS.Models.Dto;

namespace TaskDNS.Models.SQLServer
{
    ///<inheritdoc />
    public class CommandRepostiory : ICommandRepository
    {
        private CommandContext Db { get; }
        ///<inheritdoc />
        public CommandRepostiory(CommandContext db)
        {
            this.Db = db;
        }

        ///<inheritdoc />
        public void Add(Command command)
        {
           Db.Add(command);
        }

        ///<inheritdoc  />
        public void Save()
        {
            Db.SaveChanges();
        }

        ///<inheritdoc />
        public void Remove(Command command)
        {
            Db.Remove(command);
        }

        ///<inheritdoc />
        public IEnumerable<Command> AllHistory()
        {
           return Db.Commands;
        }
    }
}
